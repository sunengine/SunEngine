using System;
using System.Collections.Generic;
using SunEngine.Core.Models.Materials;

namespace SunEngine.Core.Models
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

        public string SubTitle { get; set; }

        public string Icon { get; set; }

        /// <summary>
        /// Common title of one material
        /// For example: Video, Seminar, Article...
        /// </summary>
        public string MaterialTypeTitle { get; set; }

        /// <summary>
        /// Can contain Materials
        /// </summary>
        public bool IsMaterialsContainer { get; set; }


        /// <summary>
        /// HTML description on the top of the page on Client 
        /// </summary>
        public string Header { get; set; }

        public int? ParentId { get; set; }
        public Category Parent { get; set; }

        [LinqToDB.Mapping.Association(ThisKey = "Id", OtherKey = "ParentId")]
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();

        /// <summary>
        /// Order number inside parent Category
        /// </summary>
        public int SortNumber { get; set; }

        public string LayoutName { get; set; }

        public bool IsMaterialsNameEditable { get; set; }
        
        public string MaterialsPreviewGeneratorName { get; set; }

        public MaterialsSubTitleInputType MaterialsSubTitleInputType { get; set; }
        
        public string SettingsJson { get; set; }

        /// <summary>
        /// Containing Materials
        /// </summary>
        public ICollection<Material> Materials { get; set; }

        public int? CacheSettingsId { get; set; }

        public CategoryCacheSettings CacheSettings { get; set; }
        
        public bool IsHidden { get; set; }

        public bool IsCacheContent { get; set; }
        
        public DateTime? DeletedDate { get; set; }
    }

    public enum MaterialsSubTitleInputType : short
    {
        None = 0,
        Manual = 1,
        Auto = 2
    }
}
