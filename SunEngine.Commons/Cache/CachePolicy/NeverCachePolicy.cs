using SunEngine.Commons.Cache.CacheModels;

namespace SunEngine.Commons.Cache.CachePolicy
{
    public class NeverCachePolicy : ICachePolicy
    {
        public bool CanCache(CategoryCached category, int? page = null)
        {
            return false;
        }
    }
}