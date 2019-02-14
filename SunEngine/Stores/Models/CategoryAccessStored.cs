using System.Collections.Generic;
using System.Collections.Immutable;
using SunEngine.Models;
using SunEngine.Models.Authorization;

namespace SunEngine.Stores.Models
{
    public class CategoryAccessStored
    {
        public int Id { get; }

        public int UserGroupId { get; }
        public Role Group { get; }

        public int CategoryId { get; }
        public Category Category { get; }

        public ImmutableDictionary<int, bool> CategoryOperationAccesses { get; }

        public CategoryAccessStored(CategoryAccessTmp ca)
        {
            Id = ca.Id;
            UserGroupId = ca.UserGroupId;
            Group = ca.Group;
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

        public int UserGroupId;
        public Role Group;

        public int CategoryId;
        public Category Category;

        public IDictionary<int, bool> CategoryOperationAccesses = new Dictionary<int, bool>();

        public CategoryAccessTmp(CategoryAccess categoryAccess)
        {
            Id = categoryAccess.Id;
            UserGroupId = categoryAccess.RoleId;
            Group = categoryAccess.Role;
            CategoryId = categoryAccess.CategoryId;
            Category = categoryAccess.Category;
        }
    }
}