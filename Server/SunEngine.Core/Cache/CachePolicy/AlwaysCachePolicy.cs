using SunEngine.Core.Cache.CacheModels;

namespace SunEngine.Core.Cache.CachePolicy
{
	public class AlwaysCachePolicy : ICachePolicy
	{
		public bool CanCache(CategoryCached category, RequestOptions options)
    {
      return true;
    }

		public bool CanCache(SectionServerCached component, RequestOptions options)
    {
      return true;
    }
	}
}
