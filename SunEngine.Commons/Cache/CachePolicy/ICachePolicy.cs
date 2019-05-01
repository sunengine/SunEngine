using SunEngine.Commons.Cache.CacheModels;

namespace SunEngine.Commons.Cache.CachePolicy
{
    public interface ICachePolicy
    {
        bool CanCache(CategoryCached category, int? page = null);
    }
}