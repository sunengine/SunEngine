using System.Collections.Generic;
using LinqToDB.Mapping;

namespace SunEngine.Commons.Models
{
    public enum  ChildrenType : byte { Categories = 1, Materials = 2, Mixed = 3 }
    /// <summary>
    /// Folder - для родительских категорий сожержащих разные типы категорий
    /// </summary>
    public enum CategoryContentType { Folder = 0, Forum = 1, Articles = 2, Blog = 3 }

    public class Category
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        public string Title { get; set; }

        //public CategoryContentType CategoryContentType { get; set; }
        public ChildrenType ChildrenType { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Описание HTML сверху категории
        /// </summary>
        public string Header { get; set; }
        
        public bool AreaRoot { get; set; }

        public int? ParentId { get; set; }
        public Category Parent { get; set; }

        [Association(ThisKey = "Id", OtherKey = "ParentId" )]
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();

        public int SortNumber { get; set; }

        public ICollection<Material> Materials { get; set; }

        public bool IsDeleted { get; set; }
        
        public bool IsHidden { get; set; }
        
        //public string LowerName => Name.ToLower();

    }
}
