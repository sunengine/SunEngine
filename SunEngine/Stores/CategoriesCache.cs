using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.DataBase;
using SunEngine.Stores.Models;

namespace SunEngine.Stores
{
    public interface ICategoriesCache : IMemoryCache
    {
        CategoryStored GetCategory(int id);
        CategoryStored GetCategory(string name);
        CategoryStored GetCategoryAreaRoot(CategoryStored category);
        ImmutableDictionary<string, CategoryStored> AllCategories { get; }
        CategoryStored RootCategory { get; }
        Dictionary<string, CategoryStored> GetAllCategoriesIncludeSub(string categoriesList);
    }

    public class CategoriesCache : ICategoriesCache
    {
        private readonly IDataBaseFactory dataBaseFactory;

        public CategoriesCache(IDataBaseFactory dataBaseFactory)
        {
            this.dataBaseFactory = dataBaseFactory;
        }

        private ImmutableDictionary<string, CategoryStored> _allCategories;

        public ImmutableDictionary<string, CategoryStored> AllCategories
        {
            get
            {
                if (_allCategories == null)
                {
                    Initialize();
                }

                return _allCategories;
            }
        }

        private CategoryStored _rootCategory;

        public CategoryStored RootCategory
        {
            get
            {
                if (_rootCategory == null)
                {
                    Initialize();
                }

                return _rootCategory;
            }
        }

        public CategoryStored GetCategory(int id)
        {
            return AllCategories.FirstOrDefault(x => x.Value.Id == id).Value;
        }

        public CategoryStored GetCategory(string name)
        {
            return AllCategories[name.ToLower()];
        }

        public CategoryStored GetCategoryAreaRoot(CategoryStored category)
        {
            CategoryStored current = category;
            while (!current.IsHead)
            {
                current = category.Parent;
            }

            return current;
        }

        public Dictionary<string, CategoryStored> GetAllCategoriesIncludeSub(string categoriesList)
        {
            Dictionary<string, CategoryStored> materialsCategoriesDic = new Dictionary<string, CategoryStored>();

            if (categoriesList == null) return materialsCategoriesDic;

            var categoriesNames = categoriesList.Split(',').Select(x => x.Trim());
            foreach (var name in categoriesNames)
            {
                CategoryStored category = GetCategory(name);
                var allSub = category.AllSubCategories.ToDictionary(x=>x.Name,x=>x);
                allSub.Add(category.Name, category);

                foreach (var (key, value) in allSub)
                {
                    if (!materialsCategoriesDic.ContainsKey(key))
                    {
                        materialsCategoriesDic.Add(key, value);
                    }
                }
            }

            return materialsCategoriesDic;
        }

        public void Reset()
        {
            _allCategories = null;
            _rootCategory = null;
        }

        public void Initialize()
        {
            using (var db = dataBaseFactory.CreateDb())
            {
                var categories = db.Categories.Select(x => new CategoryStored(x)).ToDictionary(x => x.Id);

                foreach (var category in categories.Values)
                {
                    category.Init1ParentAndSub(categories);
                }
                
                foreach (var category in categories.Values)
                {
                    category.Init2AllSub();
                }
                
                foreach (var category in categories.Values)
                {
                    category.Init3SetListsAndBlockEditable();
                }

                _allCategories = categories.Values.ToImmutableDictionary(x => x.Name.ToLower());
                _rootCategory = _allCategories["root"];
            }
        }

        public async Task InitializeAsync()
        {
            using (var db = dataBaseFactory.CreateDb())
            {
                var categories = await db.Categories.Select(x => new CategoryStored(x)).ToDictionaryAsync(x => x.Id);

                foreach (var category in categories.Values)
                {
                    category.Init1ParentAndSub(categories);
                }
                
                foreach (var category in categories.Values)
                {
                    category.Init2AllSub();
                }
                
                foreach (var category in categories.Values)
                {
                    category.Init3SetListsAndBlockEditable();
                }

                _allCategories = categories.Values.ToImmutableDictionary(x => x.Name.ToLower());
                _rootCategory = _allCategories["root"];
            }
        }
    }
}