using System.Collections.Generic;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.Commons.StoreModels;

namespace SunEngine.Security.Authorization
{
    public class CategoriesAuthorization
    {
        private readonly OperationKeysContainer OperationKeys;
        private readonly IAuthorizationService authorizationService;

        public CategoriesAuthorization(OperationKeysContainer operationKeysContainer,IAuthorizationService authorizationService)
        {
            this.authorizationService = authorizationService;
            OperationKeys = operationKeysContainer;
        }
        
        public List<int> GetSubCategoriesIdsCanRead(IReadOnlyDictionary<string,UserGroup> userGroups,Category categoryParent)
        {
            List<int> categories = new List<int>();
           
            foreach (var category in categoryParent.SubCategories)
            {
                if (category.IsFolder)
                {
                    var subAdd = GetSubCategoriesIdsCanRead(userGroups,category);
                    categories.AddRange(subAdd);
                }
                else
                {
                    if (authorizationService.HasAccess(userGroups, category, OperationKeys.MaterialAndMessagesRead))
                    {
                        categories.Add(category.Id);
                    }
                }
            }

            return categories;
        }

        /*public HashSet<CategoryOperations> GetOperationForCanAddMaterialsToCategoryOne(ClaimsPrincipal user,Category category)
        {
            List<OperationKey> allPossibleOperationKeys = new List<OperationKey>()
            {
                OperationKey.WriteDirectMaterial,
                OperationKey.WriteThroughModerationMaterial
            };
            var operationKeys = _authorizationService.HasAccess(user, category,allPossibleOperationKeys);
            
            HashSet<MaterialOperations> rezultOperationKeys = new HashSet<MaterialOperations>();
             
        }*/
    }
}