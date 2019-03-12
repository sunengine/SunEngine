using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Cache.CacheModels;
using SunEngine.DataBase;
using SunEngine.Utils;

namespace SunEngine.Cache
{
    public interface ICategoriesCache : IMyMemoryCache
    {
        IReadOnlyDictionary<string, SectionTypeCached> AllSectionTypes { get; }
        CategoryCached GetCategory(int id);
        CategoryCached GetCategory(string name);
        IReadOnlyDictionary<string, CategoryCached> AllCategories { get; }
        CategoryCached RootCategory { get; }
        IDictionary<string, CategoryCached> GetAllCategoriesIncludeSub(string categoriesList);
    }

    public class CategoriesCache : ICategoriesCache
    {
        private readonly IDataBaseFactory dataBaseFactory;

        public CategoriesCache(IDataBaseFactory dataBaseFactory)
        {
            this.dataBaseFactory = dataBaseFactory;
        }

        private IReadOnlyDictionary<string, SectionTypeCached> _allSectionTypes;

        public IReadOnlyDictionary<string, SectionTypeCached> AllSectionTypes
        {
            get
            {
                if (_allSectionTypes == null)
                {
                    Initialize();
                }

                return _allSectionTypes;
            }
        }

        private IReadOnlyDictionary<string, CategoryCached> _allCategories;

        public IReadOnlyDictionary<string, CategoryCached> AllCategories
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
            return AllCategories[Normalizer.Normalize(name)];
        }


        public IDictionary<string, CategoryCached> GetAllCategoriesIncludeSub(string categoriesList)
        {
            Dictionary<string, CategoryCached> materialsCategoriesDic = new Dictionary<string, CategoryCached>();

            if (categoriesList == null) return materialsCategoriesDic;

            var categoriesNames = categoriesList.Split(',').Select(x => x.Trim());
            foreach (var name in categoriesNames)
            {
                CategoryCached category = GetCategory(name);
                var allSub = category.AllSubCategories.ToDictionary(x => x.Name, x => x);
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
                _allSectionTypes = db.SectionTypes
                    .ToImmutableDictionary(x => x.Name, x => new SectionTypeCached(x));

                var categories = db.Categories.Where(x => !x.IsDeleted).Select(x => new CategoryCached(x))
                    .ToDictionary(x => x.Id);

                PrepareCategories(categories);
            }
        }

        public async Task InitializeAsync()
        {
            using (var db = dataBaseFactory.CreateDb())
            {
                _allSectionTypes = (await db.SectionTypes
                        .ToDictionaryAsync(x => x.Name, x => new SectionTypeCached(x)))
                    .ToImmutableDictionary();

                var categories = await db.Categories.Where(x => !x.IsDeleted).Select(x => new CategoryCached(x))
                    .ToDictionaryAsync(x => x.Id);

                PrepareCategories(categories);
            }
        }

        protected void PrepareCategories(Dictionary<int, CategoryCached> categories)
        {
            foreach (var category in categories.Values)
            {
                category.Init1ParentAndSub(categories);
            }

            _rootCategory = categories[1];

            var categoriesList = _rootCategory.Init2AllSub();
            categoriesList.Insert(0, _rootCategory);

            foreach (var category in categoriesList)
            {
                category.Init3ISectionType(_allSectionTypes);
            }

            _rootCategory.Init4InitSectionsRoots();
            _rootCategory.Init5PreparePaths();

            foreach (var category in categoriesList)
            {
                category.Init6SetListsAndBlockEditable();
            }

            _allCategories = categoriesList.ToImmutableDictionary(x => x.NameNormalized);
        }
    }
}