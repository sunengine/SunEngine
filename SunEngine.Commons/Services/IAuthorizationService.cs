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
    
    
    #region // old code
    /*public interface IAuthorizationService
    {
        bool HasAccess(ClaimsPrincipal user, Category category, string operationKey);

        HashSet<string> HasAccess(ClaimsPrincipal user, Category category, IList<string> operationKeys);


        #region With CategoryId
        bool HasAccess(ClaimsPrincipal user, int categoryId, string operationKey);
        
        HashSet<string> HasAccess(ClaimsPrincipal user, int categoryId, IList<string> operationKeys);
        #endregion
    }*/
    #endregion
}
