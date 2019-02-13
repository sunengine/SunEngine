using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SunEngine.Models.Authorization;
using SunEngine.Utils;

namespace SunEngine.Managers
{
    public class MyRoleManager : RoleManager<UserGroup>
    {
        public MyRoleManager(IRoleStore<UserGroup> store, IEnumerable<IRoleValidator<UserGroup>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<UserGroup>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
            KeyNormalizer = FieldNormalizer.Singleton;
        }
    }
}