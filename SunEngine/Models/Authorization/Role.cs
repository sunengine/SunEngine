using System.Collections.Generic;
using LinqToDB.Identity;

namespace SunEngine.Models.Authorization
{
    public class Role : IdentityRole<int>
    {
       


        public string Title { get; set; }

        public int SortNumber { get; set; }

        public bool IsSuper { get; set; }

        public ICollection<CategoryAccess> CategoryAccesses { get; set; }
    }
}