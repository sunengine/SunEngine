using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.DataBase;
using SunEngine.Models;
using SunEngine.Stores.CacheModels;
using SunEngine.Utils;

namespace SunEngine.Stores
{
    public interface ICategoriesCache : IMemoryCache
    {
        CategoryCached GetCategory(int id);
        CategoryCached GetCategory(string name);
        CategoryCached GetCategoryAreaRoot(CategoryCached category);
        ImmutableDictionary<string, CategoryCached> AllCategories { get; }
        CategoryCached RootCategory { get; }
        Dictionary<string, CategoryCached> GetAllCategoriesIncludeSub(string categoriesList);
    }

    public class CategoriesCache : ICategoriesCache
    {
        
        private readonly IDataBaseFactory dataBaseFactory;

        public CategoriesCache(IDataBaseFactory dataBaseFactory)
        {
            this.dataBaseFactory = dataBaseFactory;
        }

        private ImmutableDictionary<string, CategoryCached> _allCategories;

        public ImmutableDictionary<string, CategoryCached> AllCategories
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

        private CategoryCached _rootCategory;

        public CategoryCached RootCategory
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

        public CategoryCached GetCategory(int id)
        {
            return AllCategories.FirstOrDefault(x => x.Value.Id == id).Value;
        }

        public CategoryCached GetCategory(string name)
        {
            return AllCategories[FieldNormalizer.Normalize(name)];
        }

        public CategoryCached GetCategoryAreaRoot(CategoryCached category)
        {
            CategoryCached current = category;
            while (!current.AppendUrlToken)
            {
                current = category.Parent;
            }

            return current;
        }

        public Dictionary<string, CategoryCached> GetAllCategoriesIncludeSub(string categoriesList)
        {
            Dictionary<string, CategoryCached> materialsCategoriesDic = new Dictionary<string, CategoryCached>();

            if (categoriesList == null) return materialsCategoriesDic;

            var categoriesNames = categoriesList.Split(',').Select(x => x.Trim());
            foreach (var name in categoriesNames)
            {
                CategoryCached category = GetCategory(name);
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
                var categories = db.Categories.Select(x => new CategoryCached(x)).ToDictionary(x => x.Id);

                foreach (var category in categories.Values)
                {
                    category.Init1ParentAndSub(categories);
                }
                
                categories[1].Init3PreparePaths();
                
                foreach (var category in categories.Values)
                {
                    category.Init2AllSub();
                }
                
                foreach (var category in categories.Values)
                {
                    category.Init4SetListsAndBlockEditable();
                }

                _allCategories = categories.Values.ToImmutableDictionary(x => FieldNormalizer.Normalize(x.Name));
                _rootCategory = _allCategories[FieldNormalizer.Normalize(Category.RootName)];
            }
        }

        public async Task InitializeAsync()
        {
            using (var db = dataBaseFactory.CreateDb())
            {
                var categories = await db.Categories.Select(x => new CategoryCached(x)).ToDictionaryAsync(x => x.Id);

                foreach (var category in categories.Values)
                {
                    category.Init1ParentAndSub(categories);
                }
                
                foreach (var category in categories.Values)
                {
                    category.Init2AllSub();
                }
                
                categories[1].Init3PreparePaths();
                
                foreach (var category in categories.Values)
                {
                    category.Init4SetListsAndBlockEditable();
                }

                _allCategories = categories.Values.ToImmutableDictionary(x => FieldNormalizer.Normalize(x.Name));
                _rootCategory = _allCategories[FieldNormalizer.Normalize(Category.RootName)];
            }
        }
    }
}