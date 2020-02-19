using SunEngine.Core.Cache.CacheModels;

namespace SunEngine.Core.Cache.CachePolicy
{
	public class NeverCachePolicy : ICachePolicy
	{
		public bool CanCache(CategoryCached category, int? page = null)
		{
			return false;
		}

		public bool CanCache(SectionServerCached component, int? page = null)
		{
			return false;
		}
	}
}