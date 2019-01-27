using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SunEngine.Commons.Models.UserGroups;

namespace SunEngine.Commons.Services
{
    public class MyRoleManager : RoleManager<UserGroupDB>
    {
        public MyRoleManager(IRoleStore<UserGroupDB> store, IEnumerable<IRoleValidator<UserGroupDB>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<UserGroupDB>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
            KeyNormalizer = Normalizer.Singleton;
        }
    }
}