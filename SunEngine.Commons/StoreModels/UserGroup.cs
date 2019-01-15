using System.Collections.Generic;
using System.Collections.Immutable;
using SunEngine.Commons.Models.UserGroups;

namespace SunEngine.Commons.StoreModels
{
    public class UserGroupTmp
    {
        public int Id;
        public string Name;
        public string Title;
        public List<CategoryAccessTmp> CategoryAccesses = new List<CategoryAccessTmp>();

        
        public UserGroupTmp(UserGroupDB userGroupDb)
        {
            Id = userGroupDb.Id;
            Name = userGroupDb.Name;
            Title = userGroupDb.Title;
        }
    }

    public class UserGroup
    {
        public const string UserGroupRegistered = "Registered";
        public const string UserGroupUnregistered = "Unregistered";
        public const string UserGroupAdmin = "Admin";
        
        public int Id { get; }
        public string Name { get; }
        public string Title { get; }
        public ImmutableDictionary<int, CategoryAccess> CategoryAccesses { get; }

        
        public UserGroup(UserGroupTmp ug)
        {
            Id = ug.Id;
            Name = ug.Name;
            Title = ug.Title;
            CategoryAccesses = ug.CategoryAccesses
                .ToImmutableDictionary(x => x.CategoryId, x => new CategoryAccess(x));
        }
    }
}