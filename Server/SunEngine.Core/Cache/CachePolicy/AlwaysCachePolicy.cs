using SunEngine.Core.Cache.CacheModels;

namespace SunEngine.Core.Cache.CachePolicy
{
	public class AlwaysCachePolicy : ICachePolicy
	{
		public bool CanCache(CategoryCached category, int? page = null)
		{
			return !page.HasValue || page == 1;
		}

		public bool CanCache(ComponentServerCached component, int? page = null)
		{
			return !page.HasValue || page == 1;
		}
	}
}