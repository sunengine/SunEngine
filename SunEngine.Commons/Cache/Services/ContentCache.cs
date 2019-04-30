using System;
using System.Collections.Generic;
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
        private List<string> recordsKeyList = new List<string>();

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

            var invalidateCacheTime = 15;
            if (cacheOptions.Value.InvalidateCacheTime.HasValue)
            {
                invalidateCacheTime = cacheOptions.Value.InvalidateCacheTime.Value;
                if (invalidateCacheTime == 0)
                    invalidateCacheTime = int.MaxValue;
            }

            var options = new MemoryCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(invalidateCacheTime),
                PostEvictionCallbacks =
                {
                    new PostEvictionCallbackRegistration()
                    {
                        EvictionCallback = OnCacheRecordExpire
                    }
                }
            };
            
            memoryCache.Set(key, content, options);
            recordsKeyList.Add(key);
            return true;
        }

        public bool CacheContent(string key, object content, out string convertedContent)
        {
            convertedContent = WebJson.Serialize(content);
            return CacheContent(key, convertedContent);
        }

        public void InvalidateCache(int categoryId)
        {
            foreach (var key in recordsKeyList)
            {
                if (key.Contains($",{categoryId},"))
                    memoryCache.Remove(key);
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
            recordsKeyList.ForEach(x => memoryCache.Remove(x));
        }

        private int? GetCategoryId(string categoryName)
        {
            return categoriesCache.GetCategory(Normalizer.Normalize(categoryName))?.Id;
        }

        private void OnCacheRecordExpire(object key,
            object value,
            EvictionReason reason,
            object state)
        {
            recordsKeyList.Remove((string) key);
        }
    }
}