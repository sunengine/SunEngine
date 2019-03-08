using System.Collections.Generic;
using System.Linq;
using SunEngine.Models;
using SunEngine.Security.Authorization;
using SunEngine.Stores;
using SunEngine.Stores.CacheModels;

namespace SunEngine.Presenters
{
    public interface ICategoriesPresenter
    {
        CategoryInfoWithAccesses CategoryInfoWithAccessesFromCategory(
            IReadOnlyDictionary<string, RoleCached> roles);

        CategoryInfoWithAccesses CategoryInfoWithAccessesFromCategory(CategoryCached category,
            IReadOnlyDictionary<string, RoleCached> roles);
    }

    public class CategoriesPresenter : ICategoriesPresenter
    {
        protected readonly OperationKeysContainer OperationKeys;

        protected readonly IAuthorizationService authorizationService;
        protected readonly ICategoriesCache categoriesCache;
        protected readonly IRolesCache rolesCache;

        public CategoriesPresenter(IRolesCache rolesCache,
            ICategoriesCache categoriesCache,
            IAuthorizationService authorizationService,
            OperationKeysContainer operationKeysContainer)
        {
            OperationKeys = operationKeysContainer;

            this.authorizationService = authorizationService;
            this.categoriesCache = categoriesCache;
            this.rolesCache = rolesCache;
        }

        public virtual CategoryInfoWithAccesses CategoryInfoWithAccessesFromCategory(
            IReadOnlyDictionary<string, RoleCached> roles)
        {
            return CategoryInfoWithAccessesFromCategory(categoriesCache.RootCategory, roles);
        }

        public virtual CategoryInfoWithAccesses CategoryInfoWithAccessesFromCategory(CategoryCached category,
            IReadOnlyDictionary<string, RoleCached> roles)
        {
            if (!authorizationService.HasAccess(roles, category,
                    OperationKeys.MaterialAndMessagesRead) && category.Id != categoriesCache.RootCategory.Id)
            {
                return null;
            }

            CategoryInfoWithAccesses categoryInfo = new CategoryInfoWithAccesses
            {
                Id = category.Id,
                Name = category.NameNormalized,
                Title = category.Title,
                Header = category.Header,
                IsMaterialsContainer = category.IsMaterialsContainer,
                SectionType = category.SectionType,
                Path = category.Path,
                AppendUrlToken = category.AppendUrlToken,
                SortNumber = category.SortNumber,
                IsHidden = category.IsHidden,

                CategoryPersonalAccess = DetectPersonalAccesses(category, roles)
            };


            if (category.SubCategories == null) return categoryInfo;

            IEnumerable<CategoryCached> where;
            if (roles.Any(x => x.Value.Name == "Admin")) // админ может видеть все категории, в том числе и скрытые
                where = category.SubCategories;
            else
                where = category.SubCategories.Where(x => !x.IsHidden);


            if (!where.Any()) return categoryInfo;

            categoryInfo.SubCategories = new List<CategoryInfoWithAccesses>(category.SubCategories.Count);

            foreach (var child in where.OrderBy(x => x.SortNumber))
            {
                var childInfo = CategoryInfoWithAccessesFromCategory(child, roles);
                if (childInfo == null)
                {
                    continue;
                }

                categoryInfo.SubCategories.Add(childInfo);
            }

            return categoryInfo;
        }

        protected Dictionary<string, bool> DetectPersonalAccesses(CategoryCached category,
            IReadOnlyDictionary<string, RoleCached> roles)
        {
            Dictionary<string, bool> dict = new Dictionary<string, bool>(rolesCache.AllOperationKeys.Count);

            foreach (var operationKey in rolesCache.AllOperationKeys)
            {
                bool allow = authorizationService.HasAccess(roles, category,
                    operationKey.OperationKeyId);

                if (allow)
                {
                    dict[operationKey.Name] = true;
                }
            }

            return dict;
        }
    }

    public class CategoryInfoWithAccesses
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string Header { get; set; }
        public bool AppendUrlToken { get; set; }
        public int SortNumber { get; set; }
        public SectionTypeCached SectionType { get; set; }
        public string Path { get; set; }
        public bool IsMaterialsContainer { get; set; }
        public bool IsHidden { get; set; }

        public Dictionary<string, bool> CategoryPersonalAccess { get; set; }

        public List<CategoryInfoWithAccesses> SubCategories { get; set; }
    }
    
}