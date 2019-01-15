using System.Collections.Generic;
using System.Collections.Immutable;
using SunEngine.Commons.Models;
using SunEngine.Commons.Models.UserGroups;

namespace SunEngine.Commons.StoreModels
{
    public class CategoryAccessTmp
    {
        public int Id;

        public int UserGroupId;
        public UserGroupDB GroupDb;

        public int CategoryId;
        public Category Category;

        public IDictionary<int, bool> CategoryOperationAccesses = new Dictionary<int, bool>();

        public CategoryAccessTmp(CategoryAccessDB categoryAccessDb)
        {
            Id = categoryAccessDb.Id;
            UserGroupId = categoryAccessDb.UserGroupId;
            GroupDb = categoryAccessDb.GroupDb;
            CategoryId = categoryAccessDb.CategoryId;
            Category = categoryAccessDb.Category;
        }
    }
    
    public class CategoryAccess
    {
        public int Id { get; }
        
        public int UserGroupId { get; }
        public UserGroupDB GroupDb { get; }
        
        public int CategoryId { get; }
        public Category Category { get;}

        public ImmutableDictionary<int, bool> CategoryOperationAccesses { get; }

        public CategoryAccess(CategoryAccessTmp ca)
        {
            Id = ca.Id;
            UserGroupId = ca.UserGroupId;
            GroupDb = ca.GroupDb;
            CategoryId = ca.CategoryId;
            Category = ca.Category;
            CategoryOperationAccesses = ca.CategoryOperationAccesses.ToImmutableDictionary();
        }
    }
}