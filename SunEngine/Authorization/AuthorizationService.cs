using System.Collections.Generic;
using System.Linq;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.Commons.StoreModels;
using SunEngine.Stores;

namespace SunEngine.Authorization
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ICategoriesStore _categoriesStore;
        
        public AuthorizationService(ICategoriesStore categoriesStore)
        {
            this._categoriesStore = categoriesStore;
        }
        
        public bool HasAccess(IReadOnlyDictionary<string,UserGroup> userGroups, Category category, int operationKey)
        {
            if (userGroups.ContainsKey(UserGroup.UserGroupAdmin))
            {
                return true;
            }

            foreach (UserGroup userGroup in userGroups.Values)
            {
                if (GetAccessForCategory(userGroup, category, operationKey))
                {
                    return true;
                }
            }
            return false;
        }
       
        
        public HashSet<int> HasAccess(IReadOnlyDictionary<string,UserGroup> userGroups, Category category, IEnumerable<int> operationKeys)
        {
            if (userGroups.ContainsKey(UserGroup.UserGroupAdmin))
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


        private bool GetAccessForCategory(UserGroup userGroup, Category category, int operationKey)
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
        public bool HasAccess(IReadOnlyDictionary<string,UserGroup> userGroups, int categoryId, int operationKey)
        {
            return HasAccess(userGroups, _categoriesStore.GetCategory(categoryId), operationKey);
        }

        public HashSet<int> HasAccess(IReadOnlyDictionary<string,UserGroup> userGroups, int categoryId,
            IEnumerable<int> operationKeys)
        {
            return HasAccess(userGroups, _categoriesStore.GetCategory(categoryId), operationKeys);
        }
        #endregion
    }
}