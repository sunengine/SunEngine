using System.Collections.Generic;
using SunEngine.Commons.Models.Materials;

namespace SunEngine.Commons.Models
{
    /// <summary>
    /// Container for materials of any type or for Categories
    /// </summary>
    public class Category
    {
        public const string RootName = "Root";

        public int Id { get; set; }

        //[Required, MinLength(2), RegularExpression("^[a-zA-Z-]*$")]
        public string Name { get; set; }

        public string NameNormalized { get; set; }
        
        //[Required, MinLength(3)] 
        /// <summary>
        /// Title for human
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Common title of one material
        /// For example: Video, Seminar, Article...
        /// </summary>
        public string MaterialTypeTitle { get; set; }

        /// <summary>
        /// Can contain Materials
        /// </summary>
        public bool IsMaterialsContainer { get; set; }

        public string Description { get; set; }
        
        /// <summary>
        /// HTML description on the top of the page on Client 
        /// </summary>
        public string Header { get; set; }

        public bool AppendUrlToken { get; set; }
        
        public int? SectionTypeId { get; set; }
        public SectionType SectionType { get; set; }

        public int? ParentId { get; set; }
        public Category Parent { get; set; }

        [LinqToDB.Mapping.Association(ThisKey = "Id", OtherKey = "ParentId")]
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();

        /// <summary>
        /// Order number inside parent Category
        /// </summary>
        public int SortNumber { get; set; }

        /// <summary>
        /// Containing Materials
        /// </summary>
        public ICollection<Material> Materials { get; set; }

        public int? CacheSettingsId { get; set; }
        
        public CategoryCacheSettings CacheSettings { get; set; }
        
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