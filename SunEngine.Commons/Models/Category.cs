using System.Collections.Generic;
using LinqToDB.Mapping;

namespace SunEngine.Commons.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Title { get; set; }

        public bool IsMaterialsContainer { get; set; }

        [NotColumn]
        public bool IsFolder 
        {
            get => !IsMaterialsContainer;
            set => IsMaterialsContainer = !value;
        }

        public string Description { get; set; }

        /// <summary>
        /// Описание HTML сверху категории
        /// </summary>
        public string Header { get; set; }

        public bool AreaRoot { get; set; }

        public int? ParentId { get; set; }
        public Category Parent { get; set; }

        [Association(ThisKey = "Id", OtherKey = "ParentId")]
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();

        public int SortNumber { get; set; }

        public ICollection<Material> Materials { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsHidden { get; set; }
    }
}