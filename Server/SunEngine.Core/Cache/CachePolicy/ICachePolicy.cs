using SunEngine.Core.Cache.CacheModels;

namespace SunEngine.Core.Cache.CachePolicy
{
	public interface ICachePolicy
	{
		bool CanCache(CategoryCached category, RequestOptions options);
		bool CanCache(SectionServerCached component, RequestOptions options);
	}
}
