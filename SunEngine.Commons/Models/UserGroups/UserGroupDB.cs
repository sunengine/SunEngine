using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using LinqToDB.Identity;
using SunEngine.Commons.StoreModels;

namespace SunEngine.Commons.Models.UserGroups
{
    public class UserGroupDB : IdentityRole<int>
    {
        public string Title { get; set; }

        public int SortNumber { get; set; }

        public bool IsSuper { get; set; }

        public ICollection<CategoryAccessDB> CategoryAccesses { get; set; }

        
    }
}