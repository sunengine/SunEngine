using System.Collections.Generic;
using System.Collections.Immutable;
using SunEngine.Models;
using SunEngine.Models.Materials;

namespace SunEngine.Stores.Models
{
    public class CategoryStored
    {
        public int Id { get; }

        public string Name { get; }

        public string Title { get; }

        public bool IsMaterialsContainer { get; }

        public bool IsCategoriesContainer => !IsMaterialsContainer;

        public string Description { get; }

        public string Header { get; }

        public bool IsHead { get; }

        public int? ParentId { get; }
        public CategoryStored Parent { get; private set; }

        public IImmutableList<CategoryStored> SubCategories { get; private set; }

        public IImmutableList<CategoryStored> AllSubCategories { get; private set; }

        //public IReadOnlyDictionary<string, CategoryStored> AllSubCategoriesDic { get; private set; }

        protected List<CategoryStored> _subCategories { get; private set; } 

        protected List<CategoryStored> _allSubCategories { get; private set; }

        public int SortNumber { get; }

        public bool IsHidden { get; }


        protected bool initialized = false;

        public CategoryStored(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Title = category.Title;
            IsMaterialsContainer = category.IsMaterialsContainer;
            Description = category.Description;
            Header = category.Header;
            IsHead = category.IsHead;
            ParentId = category.ParentId;
            SortNumber = category.SortNumber;
            IsHidden = category.IsHidden;
            _subCategories = new List<CategoryStored>();
            _allSubCategories = new List<CategoryStored>();
        }

        

        public void Init1ParentAndSub(Dictionary<int, CategoryStored> allCategories)
        {
            if (initialized)
                return;

            if (ParentId.HasValue)
            {
                Parent = allCategories[ParentId.Value];
                Parent._subCategories.Add(this);
            }

            initialized = true;
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

        public void Init3SetListsAndBlockEditable()
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