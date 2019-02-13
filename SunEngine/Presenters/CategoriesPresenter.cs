using System.Collections.Generic;
using System.Linq;
using SunEngine.Models;
using SunEngine.Security.Authorization;
using SunEngine.Stores;
using SunEngine.Stores.Models;

namespace SunEngine.Presenters
{
    public interface ICategoriesPresenter
    {
        CategoryInfoWithAccesses CategoryInfoWithAccessesFromCategory(
            IReadOnlyDictionary<string, UserGroupStored> userGroups);

        CategoryInfoWithAccesses CategoryInfoWithAccessesFromCategory(Category category,
            IReadOnlyDictionary<string, UserGroupStored> userGroups);
    }

    public class CategoriesPresenter : ICategoriesPresenter
    {
        protected readonly OperationKeysContainer OperationKeys;

        protected readonly IAuthorizationService authorizationService;
        protected readonly ICategoriesStore categoriesStore;
        protected readonly IUserGroupStore userGroupStore;

        public CategoriesPresenter(IUserGroupStore userGroupStore,
            ICategoriesStore categoriesStore,
            IAuthorizationService authorizationService,
            OperationKeysContainer operationKeysContainer)
        {
            OperationKeys = operationKeysContainer;

            this.authorizationService = authorizationService;
            this.categoriesStore = categoriesStore;
            this.userGroupStore = userGroupStore;
        }

        public virtual CategoryInfoWithAccesses CategoryInfoWithAccessesFromCategory(
            IReadOnlyDictionary<string, UserGroupStored> userGroups)
        {
            return CategoryInfoWithAccessesFromCategory(categoriesStore.RootCategory, userGroups);
        }

        public virtual CategoryInfoWithAccesses CategoryInfoWithAccessesFromCategory(Category category,
            IReadOnlyDictionary<string, UserGroupStored> userGroups)
        {
            if (!authorizationService.HasAccess(userGroups, category,
                    OperationKeys.MaterialAndMessagesRead) && category.Id != categoriesStore.RootCategory.Id)
            {
                return null;
            }

            CategoryInfoWithAccesses categoryInfo = new CategoryInfoWithAccesses
            {
                Id = category.Id,
                Name = category.Name.ToLower(),
                Title = category.Title,
                Header = category.Header,
                IsMaterialsContainer = category.IsMaterialsContainer,
                AreaRoot = category.AreaRoot,
                SortNumber = category.SortNumber,
                IsHidden = category.IsHidden,

                CategoryPersonalAccess = DetectPersonalAccesses(category, userGroups)
            };


            if (category.SubCategories == null) return categoryInfo;

            IEnumerable<Category> where;
            if (userGroups.Any(x => x.Value.Name == "Admin")) // админ может видеть все категории, в том числе и скрытые
                where = category.SubCategories;
            else
                where = category.SubCategories.Where(x => !x.IsHidden);


            if (!where.Any()) return categoryInfo;

            categoryInfo.SubCategories = new List<CategoryInfoWithAccesses>(category.SubCategories.Count);

            foreach (var child in where.OrderBy(x => x.SortNumber))
            {
                var childInfo = CategoryInfoWithAccessesFromCategory(child, userGroups);
                if (childInfo == null)
                {
                    continue;
                }

                categoryInfo.SubCategories.Add(childInfo);
            }

            return categoryInfo;
        }

        protected Dictionary<string, bool> DetectPersonalAccesses(Category category,
            IReadOnlyDictionary<string, UserGroupStored> userGroups)
        {
            Dictionary<string, bool> dict = new Dictionary<string, bool>(userGroupStore.AllOperationKeys.Count);

            foreach (var operationKey in userGroupStore.AllOperationKeys)
            {
                bool allow = authorizationService.HasAccess(userGroups, category,
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
        public bool AreaRoot { get; set; }
        public int SortNumber { get; set; }
        public bool IsMaterialsContainer { get; set; }
        public bool IsHidden { get; set; }

        public Dictionary<string, bool> CategoryPersonalAccess { get; set; }

        public List<CategoryInfoWithAccesses> SubCategories { get; set; }
    }
}