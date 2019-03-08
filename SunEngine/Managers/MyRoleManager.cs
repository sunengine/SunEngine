using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SunEngine.Models.Authorization;
using SunEngine.Utils;

namespace SunEngine.Managers
{
    public class MyRoleManager : RoleManager<Role>
    {
        public MyRoleManager(IRoleStore<Role> store, IEnumerable<IRoleValidator<Role>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<Role>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
            KeyNormalizer = Normalizer.Singleton;
        }
    }
}