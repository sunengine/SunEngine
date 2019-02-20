using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using SunEngine.Models;

namespace SunEngine.Stores.CacheModels
{
    public class CategoryCached
    {
        public int Id { get; }

        public string Name { get; }

        public string Title { get; }

        public bool IsMaterialsContainer { get; }

        public bool IsCategoriesContainer { get; }

        public string Description { get; }

        public string Header { get; }

        public bool AppendUrlToken { get; }

        public string Path { get; private set; }

        public SectionType SectionType { get; }

        public int? ParentId { get; }
        public CategoryCached Parent { get; private set; }

        public IImmutableList<CategoryCached> SubCategories { get; private set; }

        public IImmutableList<CategoryCached> AllSubCategories { get; private set; }

        //public IReadOnlyDictionary<string, CategoryStored> AllSubCategoriesDic { get; private set; }

        protected List<CategoryCached> _subCategories { get; private set; }

        protected List<CategoryCached> _allSubCategories { get; private set; }

        public int SortNumber { get; }

        public bool IsHidden { get; }


        protected bool initialized = false;

        public CategoryCached(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Title = category.Title;
            IsMaterialsContainer = category.IsMaterialsContainer;
            IsCategoriesContainer = category.IsCategoriesContainer;
            Description = category.Description;
            Header = category.Header;
            AppendUrlToken = category.AppendUrlToken;
            ParentId = category.ParentId;
            SortNumber = category.SortNumber;
            IsHidden = category.IsHidden;
            SectionType = category.SectionType;
            _subCategories = new List<CategoryCached>();
            _allSubCategories = new List<CategoryCached>();
        }


        public void Init1ParentAndSub(Dictionary<int, CategoryCached> allCategories)
        {
            if (initialized)
                return;

            if (ParentId.HasValue)
            {
                Parent = allCategories[ParentId.Value];
                Parent._subCategories.Add(this);
            }
        }

        public void Init2AllSub()
        {
            if (initialized)
                return;

            if (!ParentId.HasValue)
                return;

            var current = Parent;
            while (current != null)
            {
                current._allSubCategories.Add(this);

                current = current.Parent;
            }
        }

        /// <summary>
        /// Должна запускаться только на Root, так как до других категорий доберётся через реккурсию
        /// </summary>
        public void Init3PreparePaths()
        {
            if (initialized)
                return;

            if (AppendUrlToken && Name != Category.RootName)
            {
                Path += "/" + Name.ToLower();
            }

            foreach (var category in _subCategories)
            {
                category.Path = Path;
            }

            foreach (var category in _subCategories)
            {
                category.Init3PreparePaths();
            }

            if (!AppendUrlToken  && Name != Category.RootName)
            {
                Path += "/" + Name.ToLower();
            }
        }

        public void Init4SetListsAndBlockEditable()
        {
            if (initialized)
                return;

            //AllSubCategoriesDic = _allSubCategories.ToImmutableDictionary(x=>x.Name,x=>x);
            AllSubCategories = _allSubCategories.ToImmutableList();
            SubCategories = _subCategories.ToImmutableList();

            _allSubCategories = null;
            _subCategories = null;

            initialized = true;
        }

        /*public void Init(CategoryStored parent, IReadOnlyList<CategoryStored> subCategories,
            IReadOnlyList<CategoryStored> allSubCategories)
        {
            if (initialized)
                return;

            this.Parent = parent;
            this.SubCategories = subCategories;
            this.AllSubCategories = allSubCategories;

            initialized = true;
        }*/
    }
}