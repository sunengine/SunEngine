using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.DataBase;
using SunEngine.Models;

namespace SunEngine.Stores
{
    public class CategoriesStore : ICategoriesStore
    {
        private readonly IDataBaseFactory dataBaseFactory;

        public CategoriesStore(IDataBaseFactory dataBaseFactory)
        {
            this.dataBaseFactory = dataBaseFactory;
        }

        private ImmutableDictionary<string, Category> _allCategories;
        
        public ImmutableDictionary<string,Category> AllCategories {
            get
            {
                if (_allCategories == null)
                {
                    Initialize();
                }

                return _allCategories;
            }
        }

        private Category _rootCategory;
        
        public Category RootCategory {
            get
            {
                if (_rootCategory == null)
                {
                    Initialize();
                }

                return _rootCategory;
            }
        }
        
        public Category GetCategory(int id)
        {
            return AllCategories.FirstOrDefault(x=>x.Value.Id == id).Value;
        }

        public Category GetCategory(string name)
        {
            return AllCategories[name.ToLower()];
        }

        public Category GetCategoryAreaRoot(Category category)
        {
            Category current = category;
            while (!current.AreaRoot)
            {
                current = category.Parent;
            }

            return current;
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

                var categories = db.Categories.ToDictionary(x=>x.Id);
                foreach (var category in categories.Values)
                {
                    if (category.ParentId.HasValue)
                    {
                        category.Parent = categories[category.ParentId.Value];
                        category.Parent.SubCategories.Add(category);
                    }
                }

                _allCategories = categories.Values.ToImmutableDictionary(x => x.Name.ToLower());
                _rootCategory = _allCategories["root"];
            }
        }

        public async Task InitializeAsync()
        {
            using (var db = dataBaseFactory.CreateDb())
            {

                var categories = await db.Categories.ToDictionaryAsync(x=>x.Id);
                foreach (var category in categories.Values)
                {
                    if (category.ParentId.HasValue)
                    {
                        category.Parent = categories[category.ParentId.Value];
                        category.Parent.SubCategories.Add(category);
                    }
                }    
                
                _allCategories = categories.Values.ToImmutableDictionary(x => x.Name.ToLower());
                _rootCategory = _allCategories["root"];
            }
        }
    }
}