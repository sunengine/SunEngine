using Microsoft.Extensions.DependencyInjection;
using SunEngine.Commons.Cache.CacheModels;
using SunEngine.Commons.DataBase;

namespace SunEngine.Commons.Cache.CachePolicy
{
    public class CustomCachePolicy : ICachePolicy

    {
        public bool CanCache(CategoryCached category, int? page = null)
        {
            if (category.CacheSettings == null)
                return true;

            return true;
        }
    }
}