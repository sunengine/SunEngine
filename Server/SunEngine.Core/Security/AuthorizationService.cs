using System.Collections.Generic;
using System.Linq;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.Cache.Services;

namespace SunEngine.Core.Security
{
	/// <summary>
	/// Main authorization service interface to check access for (Role, Category and OperationKey) triple.
	/// </summary>
	public interface IAuthorizationService
	{
		bool HasAccess(IReadOnlyDictionary<string, RoleCached> roles, CategoryCached category, int operationKey);

		HashSet<int> HasAccess(IReadOnlyDictionary<string, RoleCached> roles, CategoryCached category,
			IEnumerable<int> operationKeys);

		IList<CategoryCached> GetAllowedCategories(IReadOnlyDictionary<string, RoleCached> userGroups,
			IEnumerable<CategoryCached> categories, int operationKey);


		#region With CategoryId

		bool HasAccess(IReadOnlyDictionary<string, RoleCached> roles, int categoryId, int operationKey);

		HashSet<int> HasAccess(IReadOnlyDictionary<string, RoleCached> roles, int categoryId,
			IEnumerable<int> operationKeys);

		#endregion
	}

	/// <summary>
	/// Main authorization service to check access for (Role, Category and OperationKey) triple.
	/// </summary>
	public class AuthorizationService : IAuthorizationService
	{
		private readonly ICategoriesCache categoriesCache;

		public AuthorizationService(ICategoriesCache categoriesCache)
		{
			this.categoriesCache = categoriesCache;
		}

		public bool HasAccess(IReadOnlyDictionary<string, RoleCached> roles, CategoryCached category,
			int operationKey)
		{
			if (roles.ContainsKey(RoleNames.Admin))
				return true;

			return roles.Values.Any(role => GetAccessForCategory(role, category, operationKey));
		}


		public HashSet<int> HasAccess(IReadOnlyDictionary<string, RoleCached> roles, CategoryCached category,
			IEnumerable<int> operationKeys)
		{
			if (roles.ContainsKey(RoleNames.Admin))
				operationKeys.ToHashSet();

			HashSet<int> operationKeysReturn = new HashSet<int>();
			foreach (int operationKey in operationKeys)
				if (HasAccess(roles, category, operationKey))
					operationKeysReturn.Add(operationKey);

			return operationKeysReturn;
		}


		private bool GetAccessForCategory(RoleCached role, CategoryCached category, int operationKey)
		{
			while (category != null)
			{
				if (role.CategoryAccesses.ContainsKey(category.Id))
				{
					var categoryAccess = role.CategoryAccesses[category.Id];
					if (categoryAccess.CategoryOperationAccesses.ContainsKey(operationKey))
						return categoryAccess.CategoryOperationAccesses[operationKey];

					category = category.Parent;
				}
				else
				{
					category = category.Parent;
				}
			}

			return false;
		}

		public IList<CategoryCached> GetAllowedCategories(IReadOnlyDictionary<string, RoleCached> roles,
			IEnumerable<CategoryCached> categories, int operationKey)
		{
			return categories.Where(category => HasAccess(roles, category, operationKey)).ToList();
		}


		#region With CategoryId

		public bool HasAccess(IReadOnlyDictionary<string, RoleCached> roles, int categoryId, int operationKey)
		{
			return HasAccess(roles, categoriesCache.GetCategory(categoryId), operationKey);
		}

		public HashSet<int> HasAccess(IReadOnlyDictionary<string, RoleCached> roles, int categoryId,
			IEnumerable<int> operationKeys)
		{
			return HasAccess(roles, categoriesCache.GetCategory(categoryId), operationKeys);
		}

		#endregion
	}
}