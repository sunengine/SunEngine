using SunEngine.Core.Cache.CacheModels;

namespace SunEngine.Core.Cache.CachePolicy
{
	public class AlwaysCachePolicy : ICachePolicy
	{
		public bool CanCache(CategoryCached category, int? page = null)
    {
      return true;
    }

		public bool CanCache(SectionServerCached component, int? page = null)
    {
      return true;
    }
	}
}
