using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;

namespace SunEngine.Commons.Models.UserGroups
{
    public class CategoryAccessDB
    {
        public int Id { get; set; }
        
        public int UserGroupId { get; set; }
        public UserGroupDB GroupDb { get; set; }
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<CategoryOperationAccessDB> CategoryOperationAccesses { get; set; }

       
    }
}