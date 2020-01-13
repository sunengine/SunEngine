using System.Collections.Generic;
using System.Collections.Immutable;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Authorization;

namespace SunEngine.Core.Cache.CacheModels
{
	/// <summary>
	/// Cache for CategoryAccess for RolesCache
	/// </summary>
	public class CategoryAccessCached
	{
		public int Id { get; }

		public int RoleId { get; }
		public Role Role { get; }

		public int CategoryId { get; }
		public Category Category { get; }

		public ImmutableDictionary<int, bool> CategoryOperationAccesses { get; }

		public CategoryAccessCached(CategoryAccessTmp ca)
		{
			Id = ca.Id;
			RoleId = ca.RoleId;
			Role = ca.Role;
			CategoryId = ca.CategoryId;
			Category = ca.Category;
			CategoryOperationAccesses = ca.CategoryOperationAccesses.ToImmutableDictionary();
		}
	}


	/// <summary>
	/// This class is only need to build UserGroupStored
	/// </summary>
	public class CategoryAccessTmp
	{
		public int Id;

		public int RoleId;
		public Role Role;

		public int CategoryId;
		public Category Category;

		public IDictionary<int, bool> CategoryOperationAccesses = new Dictionary<int, bool>();

		public CategoryAccessTmp(CategoryAccess categoryAccess)
		{
			Id = categoryAccess.Id;
			RoleId = categoryAccess.RoleId;
			Role = categoryAccess.Role;
			CategoryId = categoryAccess.CategoryId;
			Category = categoryAccess.Category;
		}
	}
}