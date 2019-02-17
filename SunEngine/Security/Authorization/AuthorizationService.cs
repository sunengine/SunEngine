using System.Collections.Generic;
using System.Linq;
using SunEngine.Models;
using SunEngine.Stores;
using SunEngine.Stores.Models;

namespace SunEngine.Security.Authorization
{
    public interface IAuthorizationService
    {
        bool HasAccess(IReadOnlyDictionary<string, RoleStored> roles, CategoryStored category, int operationKey);

        HashSet<int> HasAccess(IReadOnlyDictionary<string, RoleStored> roles, CategoryStored category,
            IEnumerable<int> operationKeys);

        
        IList<CategoryStored> GetAllowedCategories(IReadOnlyDictionary<string, RoleStored> userGroups, IEnumerable<CategoryStored> categories, int operationKey);
        

        #region With CategoryId

        bool HasAccess(IReadOnlyDictionary<string, RoleStored> roles, int categoryId, int operationKey);

        HashSet<int> HasAccess(IReadOnlyDictionary<string, RoleStored> roles, int categoryId,
            IEnumerable<int> operationKeys);

        #endregion
    }


    public class AuthorizationService : IAuthorizationService
    {
        private readonly ICategoriesCache categoriesCache;

        public AuthorizationService(ICategoriesCache categoriesCache)
        {
            this.categoriesCache = categoriesCache;
        }

        public bool HasAccess(IReadOnlyDictionary<string, RoleStored> roles, CategoryStored category,
            int operationKey)
        {
            if (roles.ContainsKey(RoleNames.Admin))
                return true;

            return roles.Values.Any(role => GetAccessForCategory(role, category, operationKey));
        }


        public HashSet<int> HasAccess(IReadOnlyDictionary<string, RoleStored> roles, CategoryStored category,
            IEnumerable<int> operationKeys)
        {
            if (roles.ContainsKey(RoleNames.Admin))
            {
                operationKeys.ToHashSet();
            }

            HashSet<int> operationKeysReturn = new HashSet<int>();
            foreach (int operationKey in operationKeys)
            {
                if (HasAccess(roles, category, operationKey))
                {
                    operationKeysReturn.Add(operationKey);
                }
            }

            return operationKeysReturn;
        }


        private bool GetAccessForCategory(RoleStored role, CategoryStored category, int operationKey)
        {
            while (category != null)
            {
                if (role.CategoryAccesses.ContainsKey(category.Id))
                {
                    var categoryAccess = role.CategoryAccesses[category.Id];
                    if (categoryAccess.CategoryOperationAccesses.ContainsKey(operationKey))
                    {
                        return categoryAccess.CategoryOperationAccesses[operationKey];
                    }

                    category = category.Parent;
                }
                else
                {
                    category = category.Parent;
                }
            }

            return false;
        }

        public IList<CategoryStored> GetAllowedCategories(IReadOnlyDictionary<string, RoleStored> roles,
            IEnumerable<CategoryStored> categories, int operationKey)
        {
            return categories.Where(category => HasAccess(roles, category, operationKey)).ToList();
        }


        #region With CategoryId

        public bool HasAccess(IReadOnlyDictionary<string, RoleStored> roles, int categoryId, int operationKey)
        {
            return HasAccess(roles, categoriesCache.GetCategory(categoryId), operationKey);
        }

        public HashSet<int> HasAccess(IReadOnlyDictionary<string, RoleStored> roles, int categoryId,
            IEnumerable<int> operationKeys)
        {
            return HasAccess(roles, categoriesCache.GetCategory(categoryId), operationKeys);
        }

        #endregion
    }
}