using System.Collections.Generic;
using System.Collections.Immutable;
using SunEngine.Models.Authorization;

namespace SunEngine.Stores.Models
{
    public class UserGroupStored
    {
        public const string UserGroupRegistered = "Registered";
        public const string UserGroupUnregistered = "Unregistered";
        public const string UserGroupAdmin = "Admin";

        public int Id { get; }
        public string Name { get; }
        public string Title { get; }
        public ImmutableDictionary<int, CategoryAccessStored> CategoryAccesses { get; }


        public UserGroupStored(UserGroupTmp ug)
        {
            Id = ug.Id;
            Name = ug.Name;
            Title = ug.Title;
            CategoryAccesses = ug.CategoryAccesses
                .ToImmutableDictionary(x => x.CategoryId, x => new CategoryAccessStored(x));
        }
    }


    /// <summary>
    /// This class is only need to build UserGroupStored
    /// </summary>
    public class UserGroupTmp
    {
        public int Id;
        public string Name;
        public string Title;
        public List<CategoryAccessTmp> CategoryAccesses = new List<CategoryAccessTmp>();


        public UserGroupTmp(UserGroup userGroup)
        {
            Id = userGroup.Id;
            Name = userGroup.Name;
            Title = userGroup.Title;
        }
    }
}