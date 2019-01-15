using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using LinqToDB.Identity;

namespace SunEngine.Commons.Models.UserGroups
{
    public class UserGroupDB : IdentityRole<int>
    {
       
        
        public string Title { get; set; }

        public ICollection<CategoryAccessDB> CategoryAccesses { get; set; }

    }
}