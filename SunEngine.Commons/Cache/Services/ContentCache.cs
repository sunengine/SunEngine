using System;
using System.Collections.Generic;
using System.Linq;
using Flurl.Util;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SunEngine.Commons.Configuration.Options;
using SunEngine.Commons.Utils;

namespace SunEngine.Commons.Cache.Services
{
    public interface IContentCache
    {
        string GetContent(string key);
        bool CacheContent(string key, string content);
        bool CacheContent(string key, object content, out string convertedContent);
        void InvalidateCache(int categoryId);
        void InvalidateCache(string categoryName);
        void Reset();
    }
    
    public class CategoryContentCache : IContentCache
    {
        private ICategoriesCache categoriesCache;
        private IMemoryCache memoryCache;
        private IOptions<CacheOptions> cacheOptions;

        public CategoryContentCache(ICategoriesCache categoriesCache,
            IMemoryCache memoryCache,
            IOptions<CacheOptions> cacheOptions)
        {
            this.categoriesCache = categoriesCache;
            this.memoryCache = memoryCache;
            this.cacheOptions = cacheOptions;
        }

        public string GetContent(string key)
        {
            return memoryCache.Get<string>(key);
        }

        public bool CacheContent(string key, string content)
        {
            if (string.IsNullOrEmpty(key)
                || string.IsNullOrEmpty(content))
                return false;

            var invalidateCacheTime = 10;
            if (cacheOptions.Value.InvalidateCacheTime.HasValue)
            {
                invalidateCacheTime = cacheOptions.Value.InvalidateCacheTime.Value;
            }
            
            memoryCache.Set(key, content, TimeSpan.FromMinutes(invalidateCacheTime));
            return true;
        }

        public bool CacheContent(string key, object content, out string convertedContent)
        {
            convertedContent = WebJson.Serialize(content);
            return CacheContent(key, convertedContent);
        }

        public void InvalidateCache(int categoryId)
        {
            var keyValuePairs = memoryCache.ToKeyValuePairs();
            foreach (var pair in keyValuePairs)
            {
                if (pair.Key.Contains($",{categoryId},"))
                    memoryCache.Remove(pair.Key);
            }
        }

        public void InvalidateCache(string categoryName)
        {
            var categoryId = GetCategoryId(categoryName);
            if (categoryId == null)
                return;

            InvalidateCache((int) categoryId);
        }

        public void Reset()
        {
            memoryCache.ToKeyValuePairs()
                .ToList().ForEach(x => memoryCache.Remove(x.Key));
        }

        private int? GetCategoryId(string categoryName)
        {
            return categoriesCache.GetCategory(Normalizer.Normalize(categoryName))?.Id;
        }
    }
}