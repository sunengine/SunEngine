using System.Collections.Generic;

namespace SunEngine.Models.Authorization
{
    public class CategoryAccess
    {
        public int Id { get; set; }
        
        public int UserGroupId { get; set; }
        public UserGroup Group { get; set; }
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<CategoryOperationAccess> CategoryOperationAccesses { get; set; }

       
    }
}