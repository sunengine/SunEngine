using SunEngine.Core.Cache.CacheModels;

namespace SunEngine.Core.Cache.CachePolicy
{
	public class NeverCachePolicy : ICachePolicy
	{
		public bool CanCache(CategoryCached category, RequestOptions options)
		{
			return false;
		}

		public bool CanCache(SectionServerCached component, RequestOptions options)
		{
			return false;
		}
	}
}
