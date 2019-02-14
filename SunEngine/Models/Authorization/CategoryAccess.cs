using System.Collections.Generic;

namespace SunEngine.Models.Authorization
{
    public class CategoryAccess
    {
        public int Id { get; set; }
        
        public int RoleId { get; set; }
        public Role Role { get; set; }
        
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public ICollection<CategoryOperationAccess> CategoryOperationAccesses { get; set; }

       
    }
}