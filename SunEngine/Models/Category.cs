using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using LinqToDB.Mapping;
using SunEngine.Models.Materials;

namespace SunEngine.Models
{
    public class Category
    {
        public const string RootName = "Root";


        public int Id { get; set; }

        //[Required, MinLength(2), RegularExpression("^[a-zA-Z-]*$")]
        public string Name { get; set; }

        //[Required, MinLength(3)] 
        public string Title { get; set; }

        public bool IsMaterialsContainer { get; set; }

        public string Description { get; set; }
        
        //public int? SectionTypeId { get; set; }
        //public SectionType SectionType { get; set; }

        /// <summary>
        /// Описание HTML сверху категории
        /// </summary>
        public string Header { get; set; }

        public bool AppendUrlToken { get; set; }
        
        public int? SectionTypeId { get; set; }
        public SectionType SectionType { get; set; }

        public int? ParentId { get; set; }
        public Category Parent { get; set; }

        [LinqToDB.Mapping.Association(ThisKey = "Id", OtherKey = "ParentId")]
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();

        public int SortNumber { get; set; }

        public ICollection<Material> Materials { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsHidden { get; set; }
        
        public bool IsCacheContent { get; set; }
    }

    public class SectionType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
    }

    public static class SectionTypeNames
    {
        public const string Forum = "Forum";
        public const string Blog = "Blog";
        public const string Articles = "Articles";
    }
}