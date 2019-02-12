using System.Collections.Generic;
using SunEngine.Models;
using SunEngine.Stores.Models;

namespace SunEngine.Security.Authorization
{
    public interface IAuthorizationService
    {
        bool HasAccess(IReadOnlyDictionary<string,UserGroupStored> userGroups, Category category, int operationKey);

        HashSet<int> HasAccess(IReadOnlyDictionary<string,UserGroupStored> userGroups, Category category, IEnumerable<int> operationKeys);


        #region With CategoryId
        bool HasAccess(IReadOnlyDictionary<string,UserGroupStored> userGroups, int categoryId, int operationKey);
        
        HashSet<int> HasAccess(IReadOnlyDictionary<string,UserGroupStored> userGroups, int categoryId, IEnumerable<int> operationKeys);
        #endregion
    }
}
