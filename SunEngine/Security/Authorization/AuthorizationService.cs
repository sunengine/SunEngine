using System.Collections.Generic;
using System.Linq;
using SunEngine.Models;
using SunEngine.Stores;
using SunEngine.Stores.Models;

namespace SunEngine.Security.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ICategoriesStore categoriesStore;
        
        public AuthorizationService(ICategoriesStore categoriesStore)
        {
            this.categoriesStore = categoriesStore;
        }
        
        public bool HasAccess(IReadOnlyDictionary<string,UserGroupStored> userGroups, Category category, int operationKey)
        {
            if (userGroups.ContainsKey(UserGroupStored.UserGroupAdmin))
            {
                return true;
            }

            foreach (UserGroupStored userGroup in userGroups.Values)
            {
                if (GetAccessForCategory(userGroup, category, operationKey))
                {
                    return true;
                }
            }
            return false;
        }
       
        
        public HashSet<int> HasAccess(IReadOnlyDictionary<string,UserGroupStored> userGroups, Category category, IEnumerable<int> operationKeys)
        {
            if (userGroups.ContainsKey(UserGroupStored.UserGroupAdmin))
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


        private bool GetAccessForCategory(UserGroupStored userGroup, Category category, int operationKey)
        {
            while (category != null)
            {
                if (userGroup.CategoryAccesses.ContainsKey(category.Id))
                {
                    var categoryAccess = userGroup.CategoryAccesses[category.Id];
                    if (categoryAccess.CategoryOperationAccesses.ContainsKey(operationKey))
                    {
                        return categoryAccess.CategoryOperationAccesses[operationKey];
                    }
                    else
                    {
                        category = category.Parent;
                    }
                }
                else
                {
                    category = category.Parent;
                }
            }
            return false;
        }

        
        
        #region With CategoryId
        public bool HasAccess(IReadOnlyDictionary<string,UserGroupStored> userGroups, int categoryId, int operationKey)
        {
            return HasAccess(userGroups, categoriesStore.GetCategory(categoryId), operationKey);
        }

        public HashSet<int> HasAccess(IReadOnlyDictionary<string,UserGroupStored> userGroups, int categoryId,
            IEnumerable<int> operationKeys)
        {
            return HasAccess(userGroups, categoriesStore.GetCategory(categoryId), operationKeys);
        }
        #endregion
    }
}