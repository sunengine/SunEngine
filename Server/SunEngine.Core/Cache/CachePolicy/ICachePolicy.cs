using SunEngine.Core.Cache.CacheModels;

namespace SunEngine.Core.Cache.CachePolicy
{
	public interface ICachePolicy
	{
		bool CanCache(CategoryCached category, int? page = null);
		bool CanCache(ComponentServerCached component, int? page = null);
	}
}