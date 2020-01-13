using System.Collections.Immutable;
using System.Linq;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models.Authorization;
using SunEngine.Core.Security;

namespace SunEngine.Core.Cache.Services
{
	public interface IRolesCache
	{
		IImmutableList<OperationKeyCached> AllOperationKeys { get; }
		RoleCached GetRole(string name);
		IImmutableDictionary<string, RoleCached> AllRoles { get; }
		RoleCached AdminRole { get; }
		string CheckAndSetRoles(string roles);
		void Initialize();
	}

	/// <summary>
	/// Quick access roles cache for authorization system (AuthorizationService)
	/// </summary>
	public class RolesCache : IRolesCache
	{
		protected readonly IDataBaseFactory dataBaseFactory;

		public IImmutableList<OperationKeyCached> AllOperationKeys { get; protected set; }
		public IImmutableDictionary<string, RoleCached> AllRoles { get; protected set; }
		public RoleCached AdminRole { get; protected set; }


		public RolesCache(IDataBaseFactory dataBaseFactory)
		{
			this.dataBaseFactory = dataBaseFactory;
			Initialize();
		}

		public RoleCached GetRole(string name)
		{
			return AllRoles.TryGetValue(name, out var ret) ? ret : null;
		}

		public virtual string CheckAndSetRoles(string roles)
		{
			if (string.IsNullOrWhiteSpace(roles))
				return string.Join(',', RoleNames.Unregistered, RoleNames.Registered);

			var rolesNames = roles.Split(',').Select(x => x.Trim()).ToList()
				.Where(x => AllRoles.ContainsKey(x));
			return string.Join(',', rolesNames);
		}

		public void Initialize()
		{
			using var db = dataBaseFactory.CreateDb();
			var roles = db.Roles.Select(x => new RoleTmp(x)).ToDictionary(x => x.Id);

			AllOperationKeys = db.OperationKeys.Select(x => new OperationKeyCached(x)).ToImmutableList();


			var categoryAccesses = db.CategoryAccess.Select(x => new CategoryAccessTmp(x))
				.ToDictionary(x => x.Id);

			foreach (CategoryOperationAccess categoryOperationAccess in db.CategoryOperationAccess.ToList())
			{
				categoryAccesses[categoryOperationAccess.CategoryAccessId].CategoryOperationAccesses
					.Add(categoryOperationAccess.OperationKeyId, categoryOperationAccess.Access);
			}

			foreach (var categoryAccess in categoryAccesses.Values)
			{
				roles[categoryAccess.RoleId].CategoryAccesses
					.Add(categoryAccess);
			}

			AllRoles = roles.Values.ToImmutableDictionary(x => x.Name, x => new RoleCached(x));
			AdminRole = AllRoles[RoleNames.Admin];
		}
	}
}