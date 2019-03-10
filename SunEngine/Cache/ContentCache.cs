using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.Utils;

namespace SunEngine.Stores
{
    public interface IContentCache : IMemoryCache
    {
        string GetContent(string key);
        bool IsCached(int categoryId);
        bool IsCached(string categoryName);
        bool CacheContent(string key, string content);
        bool CacheContent(string key, object content, out string convertedContent);
        void InvalidateCache(int categoryId);
        void InvalidateCache(string categoryName);
    }

    public class CategoryContentCache : IContentCache
    {
        private Dictionary<string, string> allContent =
            new Dictionary<string, string>();

        private ICategoriesCache categoriesCache;

        public CategoryContentCache(ICategoriesCache categoriesCache)
        {
            this.categoriesCache = categoriesCache;
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        public Task InitializeAsync()
        {
            throw new System.NotImplementedException();
        }

        public void Reset()
        {
            allContent.Clear();
        }

        public string GetContent(string key)
        {
            allContent.TryGetValue(key, out var content);
            return content;
        }

        public bool IsCached(int categoryId)
        {
            foreach (var key in allContent.Keys)
            {
                if (key.Contains($",{categoryId},"))
                    return true;
            }

            return false;
        }

        public bool IsCached(string categoryName)
        {
            var categoryId = GetCategoryId(categoryName);
            return categoryId != null && IsCached((int)categoryId);
        }

        public bool CacheContent(string key, string content)
        {
            if (string.IsNullOrEmpty(key)
                || string.IsNullOrEmpty(content))
                return false;

            allContent[key] = content;
            return true;
        }

        public bool CacheContent(string key, object content, out string convertedContent)
        {
            convertedContent = WebJson.Serialize(content);
            return CacheContent(key, convertedContent);
        }

        public void InvalidateCache(int categoryId)
        {
            allContent.Keys.Where(x => x.Contains($",{categoryId},"))
                .ToList().ForEach(x => allContent.Remove(x));
        }

        public void InvalidateCache(string categoryName)
        {
            var categoryId = GetCategoryId(categoryName);
            if (categoryId == null)
                return;

            InvalidateCache((int)categoryId);
        }

        private int? GetCategoryId(string categoryName)
        {
            return categoriesCache.GetCategory(FieldNormalizer.Normalize(categoryName))?.Id;
        }
    }
}