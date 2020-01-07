﻿using System;
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
        /// <summary>
        /// Name id to browser routes and to find material by name feature
        /// </summary>
        public string Name { get; set; }

        public string NameNormalized { get; set; }

        //[Required, MinLength(3)]
        /// <summary>
        /// Title for human
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Sub title for human
        /// </summary>
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
        /// <summary>
        /// Parent category
        /// </summary>
        public Category Parent { get; set; }

        [LinqToDB.Mapping.Association(ThisKey = "Id", OtherKey = "ParentId")]
        public ICollection<Category> SubCategories { get; set; } = new List<Category>();

        /// <summary>
        /// Order number inside parent Category
        /// </summary>
        public int SortNumber { get; set; }

        public string LayoutName { get; set; }

        /// <summary>
        /// Allow admin to edit material name
        /// </summary>
        public bool IsMaterialsNameEditable { get; set; }

        /// <summary>
        /// Allow users to edit subtitle
        /// </summary>
        public bool IsMaterialsSubTitleEditable { get; set; }

        /// <summary>
        /// Setting json string to use on client to allow some visual or behavior settings apply
        /// </summary>
        public string SettingsJson { get; set; }

        /// <summary>
        /// Containing Materials
        /// </summary>
        public ICollection<Material> Materials { get; set; }

        public int? CacheSettingsId { get; set; }

        public bool IsHidden { get; set; }

        /// <summary>
        /// If true engine will cache category list if config cache policy set to Custom
        /// </summary>
        public bool IsCacheContent { get; set; }

        /// <summary>
        /// If set material was deleted with this date
        /// </summary>
        public DateTime? DeletedDate { get; set; }
    }
}
