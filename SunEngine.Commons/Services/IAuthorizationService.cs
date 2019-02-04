using System.Collections.Generic;
using SunEngine.Commons.Models;
using SunEngine.Commons.StoreModels;

namespace SunEngine.Commons.Services
{
    public interface IAuthorizationService
    {
        bool HasAccess(IReadOnlyDictionary<string,UserGroup> userGroups, Category category, int operationKey);

        HashSet<int> HasAccess(IReadOnlyDictionary<string,UserGroup> userGroups, Category category, IEnumerable<int> operationKeys);


        #region With CategoryId
        bool HasAccess(IReadOnlyDictionary<string,UserGroup> userGroups, int categoryId, int operationKey);
        
        HashSet<int> HasAccess(IReadOnlyDictionary<string,UserGroup> userGroups, int categoryId, IEnumerable<int> operationKeys);
        #endregion
    }
}
