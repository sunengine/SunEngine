using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Newtonsoft.Json.Linq;
using SunEngine.Core.Models;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Cache.CacheModels
{
    /// <summary>
    /// Cache of category for CategoriesCache
    /// </summary>
    public class CategoryCached
    {
        public int Id { get; }

        public string Name { get; }

        public string NameNormalized { get; }

        public string Title { get; }

        public string SubTitle { get; }

        public string Icon { get; }

        public bool IsMaterialsContainer { get; }

        public string Header { get; }

        public CategoryCached SectionRoot { get; private set; }

        public bool IsMaterialsNameEditable { get; }
        
        public string MaterialsPreviewGeneratorName { get; }
        
        public MaterialsSubTitleInputType MaterialsSubTitleInputType { get; }

        public JRaw SettingsJson { get; }

        public int? ParentId { get; }
        public CategoryCached Parent { get; private set; }

        public IImmutableList<CategoryCached> SubCategories { get; private set; }

        public IImmutableList<CategoryCached> AllSubCategories { get; private set; }

        protected List<CategoryCached> _subCategories { get; private set; }

        protected List<CategoryCached> _allSubCategories { get; private set; }

        public int SortNumber { get; }

        public string LayoutName { get; }

        public bool IsHidden { get; }

        public bool IsCacheContent { get; }


        protected bool initialized = false;

        public CategoryCached(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            NameNormalized = category.NameNormalized;
            Title = category.Title;
            IsMaterialsContainer = category.IsMaterialsContainer;
            SubTitle = category.SubTitle;
            Icon = category.Icon;
            Header = category.Header;
            IsMaterialsNameEditable = category.IsMaterialsNameEditable;
            MaterialsSubTitleInputType = category.MaterialsSubTitleInputType;
            MaterialsPreviewGeneratorName = category.MaterialsPreviewGeneratorName;
            SettingsJson = SunJson.MakeJRow(category.SettingsJson);
            ParentId = category.ParentId;
            SortNumber = category.SortNumber;
            LayoutName = category.LayoutName;
            IsHidden = category.IsHidden;
            IsCacheContent = category.IsCacheContent;

            _subCategories = new List<CategoryCached>();
            _allSubCategories = new List<CategoryCached>();
        }

        public void Init1ParentAndSub(Dictionary<int, CategoryCached> allCategories)
        {
            if (initialized)
                return;
            
            if (ParentId.HasValue && allCategories.ContainsKey(ParentId.Value))
            {
                Parent = allCategories[ParentId.Value];
                Parent._subCategories.Add(this);
            }
        }

        public List<CategoryCached> Init2AllSub()
        {
            if (initialized)
                return null;
            foreach (var category in _subCategories)
            {
                var list = category.Init2AllSub();
                
                foreach (var sub in list)
                    _allSubCategories.Add(sub);
            }

            var rez = _allSubCategories.ToList();
            rez.Add(this);
            return rez;
        }


        public void Init3InitSectionsRoots(CategoryCached sectionRoot = null)
        {
            if (initialized)
                return;
            if (LayoutName != null)
                sectionRoot = this;

            SectionRoot = sectionRoot;
            
            foreach (var category in _subCategories)
                category.Init3InitSectionsRoots(sectionRoot);
        }


        public void Init4SetListsAndBlockEditable()
        {
            if (initialized)
                return;
            
            AllSubCategories = _allSubCategories.ToImmutableList();
            SubCategories = _subCategories.ToImmutableList();
            _allSubCategories = null;
            _subCategories = null;
            initialized = true;
        }
    }
}
