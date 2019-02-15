using System.Collections.Generic;
using System.Linq;
using SunEngine.Models;
using SunEngine.Stores;
using SunEngine.Stores.Models;

namespace SunEngine.Security.Authorization
{
    public interface IAuthorizationService
    {
        bool HasAccess(IReadOnlyDictionary<string, RoleStored> userGroups, Category category, int operationKey);

        HashSet<int> HasAccess(IReadOnlyDictionary<string, RoleStored> userGroups, Category category,
            IEnumerable<int> operationKeys);


        #region With CategoryId

        bool HasAccess(IReadOnlyDictionary<string, RoleStored> userGroups, int categoryId, int operationKey);

        HashSet<int> HasAccess(IReadOnlyDictionary<string, RoleStored> userGroups, int categoryId,
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

        public bool HasAccess(IReadOnlyDictionary<string, RoleStored> userGroups, Category category,
            int operationKey)
        {
            if (userGroups.ContainsKey(RoleNames.Admin))
            {
                return true;
            }

            foreach (RoleStored userGroup in userGroups.Values)
            {
                if (GetAccessForCategory(userGroup, category, operationKey))
                {
                    return true;
                }
            }

            return false;
        }


        public HashSet<int> HasAccess(IReadOnlyDictionary<string, RoleStored> userGroups, Category category,
            IEnumerable<int> operationKeys)
        {
            if (userGroups.ContainsKey(RoleNames.Admin))
            {
                operationKeys.ToHashSet();
            }

            HashSet<int> operationKeysReturn = new HashSet<int>();
            foreach (int operationKey in operationKeys)
            {
                if (HasAccess(userGroups, category, operationKey))
                {
                    operationKeysReturn.Add(operationKey);
                }
            }

            return operationKeysReturn;
        }


        private bool GetAccessForCategory(RoleStored role, Category category, int operationKey)
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


        #region With CategoryId

        public bool HasAccess(IReadOnlyDictionary<string, RoleStored> userGroups, int categoryId, int operationKey)
        {
            return HasAccess(userGroups, categoriesCache.GetCategory(categoryId), operationKey);
        }

        public HashSet<int> HasAccess(IReadOnlyDictionary<string, RoleStored> userGroups, int categoryId,
            IEnumerable<int> operationKeys)
        {
            return HasAccess(userGroups, categoriesCache.GetCategory(categoryId), operationKeys);
        }

        #endregion
    }
}