using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Cache.Services
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
		private IOptionsMonitor<CacheOptions> cacheOptions;
		private Dictionary<string, bool> recordsKeys = new Dictionary<string, bool>();
		private static object syncObject = new object();

		public CategoryContentCache(ICategoriesCache categoriesCache,
			IMemoryCache memoryCache,
			IOptionsMonitor<CacheOptions> cacheOptions)
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

			var invalidateCacheTime = cacheOptions.CurrentValue.InvalidateCacheTime;
			if (invalidateCacheTime <= 0)
				invalidateCacheTime = int.MaxValue;

			var options = new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(invalidateCacheTime),
				PostEvictionCallbacks =
				{
					new PostEvictionCallbackRegistration
					{
						EvictionCallback = OnCacheRecordExpire
					}
				}
			};

			lock (syncObject)
			{
				memoryCache.Set(key, content, options);
				recordsKeys.TryAdd(key, true);
			}

			return true;
		}

		public bool CacheContent(string key, object content, out string convertedContent)
		{
			convertedContent = SunJson.Serialize(content);
			return CacheContent(key, convertedContent);
		}

		public void InvalidateCache(int categoryId)
		{
			lock (syncObject)
				foreach (var key in recordsKeys.Keys)
					if (key.Contains($",{categoryId},"))
					{
						memoryCache.Remove(key);
						recordsKeys.Remove(key, out _);
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
			lock (syncObject)
			{
				foreach (var recordsKey in recordsKeys.Keys)
					memoryCache.Remove(recordsKey);

				recordsKeys.Clear();
			}
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
			lock (syncObject)
				recordsKeys.Remove((string) key, out _);
		}
	}
}