using SunEngine.Core.Cache.CacheModels;

namespace SunEngine.Core.Cache.CachePolicy
{
	public class CustomCachePolicy : ICachePolicy
	{
		public bool CanCache(CategoryCached category, RequestOptions options)
		{
			return (!options.PageNumber.HasValue || options.PageNumber.Value == 1) && category.IsCacheContent;
		}

		public bool CanCache(SectionServerCached component, RequestOptions options)
		{
			return (!options.PageNumber.HasValue || options.PageNumber.Value == 1) && component.IsCacheData;
		}
	}
}
