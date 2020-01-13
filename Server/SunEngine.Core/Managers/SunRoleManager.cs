using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SunEngine.Core.Models.Authorization;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Managers
{
	public class SunRoleManager : RoleManager<Role>
	{
		public SunRoleManager(IRoleStore<Role> store, IEnumerable<IRoleValidator<Role>> roleValidators,
			ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<Role>> logger) : base(
			store, roleValidators, keyNormalizer, errors, logger)
		{
			KeyNormalizer = NormalizerLookup.Instance;
		}
	}
}