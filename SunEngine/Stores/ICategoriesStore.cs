using System.Collections.Immutable;
using SunEngine.Models;

namespace SunEngine.Stores
{
    public interface ICategoriesStore : IMemoryStore
    {
        Category GetCategory(int id);
        Category GetCategory(string name);
        Category GetCategoryAreaRoot(Category category);
        ImmutableDictionary<string,Category> AllCategories { get; }
        Category RootCategory { get; }
    }
}