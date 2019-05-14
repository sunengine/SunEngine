using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using SunEngine.Core.Models;

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

        public bool IsMaterialsContainer { get; }

        public string Description { get; }

        public string Header { get; }

        public CategoryCached SectionRoot { get; private set; }

        public int? SectionTypeId { get; private set; }

        public SectionTypeCached SectionType { get; private set; }
        
        public string SettingsJson { get; private set; }

        public int? ParentId { get; }
        public CategoryCached Parent { get; private set; }
        
        public int? CacheSettingsId { get; set; }
        
        public CategoryCacheSettings CacheSettings { get; set; }

        public IImmutableList<CategoryCached> SubCategories { get; private set; }

        public IImmutableList<CategoryCached> AllSubCategories { get; private set; }
        
        protected List<CategoryCached> _subCategories { get; private set; }

        protected List<CategoryCached> _allSubCategories { get; private set; }

        public int SortNumber { get; }
        
        public string LayoutName { get; private set; }

        public bool IsHidden { get; }
        
        public bool IsCacheContent { get; private set; }


        protected bool initialized = false;

        public CategoryCached(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            NameNormalized = category.NameNormalized;
            Title = category.Title;
            IsMaterialsContainer = category.IsMaterialsContainer;
            Description = category.Description;
            Header = category.Header;
            SectionTypeId = category.SectionTypeId;
            SettingsJson = category.SettingsJson;
            ParentId = category.ParentId;
            CacheSettingsId = category.CacheSettingsId;
            CacheSettings = category.CacheSettings;
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
                {
                    _allSubCategories.Add(sub);
                }
            }

            var rez = _allSubCategories.ToList();
            rez.Add(this);
            
            return rez;
        }

        public void Init3ISectionType(IReadOnlyDictionary<string, SectionTypeCached> sectionTypes)
        {
            if (SectionTypeId.HasValue)
                SectionType = sectionTypes.Values.FirstOrDefault(x => x.Id == SectionTypeId.Value);
        }


        public void Init4InitSectionsRoots(CategoryCached sectionRoot = null)
        {
            if (initialized)
                return;

            if (SectionType != null)
                sectionRoot = this;

            SectionRoot = sectionRoot;

            foreach (var category in _subCategories)
            {
                category.Init4InitSectionsRoots(sectionRoot);
            }
        }
        

        public void Init5SetListsAndBlockEditable()
        {
            if (initialized)
                return;

            AllSubCategories = _allSubCategories.ToImmutableList();
            SubCategories = _subCategories.ToImmutableList();

            _allSubCategories = null;
            _subCategories = null;

            initialized = true;
        }

        public bool IsDescriptionEditable()
        {
            if (SectionRoot == null)
                return false;

            if (SectionRoot.SectionType == null)
                throw new Exception("Impossible");

            return SectionRoot.SectionType.Name == SectionTypeNames.Articles;
        }
    }

    public class SectionTypeCached
    {
        public int Id { get; }
        public string Name { get; }
        public string Title { get; }

        public SectionTypeCached(SectionType sectionType)
        {
            Id = sectionType.Id;
            Name = sectionType.Name;
            Title = sectionType.Title;
        }
    }
}