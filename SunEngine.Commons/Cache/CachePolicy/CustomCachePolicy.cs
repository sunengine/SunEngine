using Microsoft.Extensions.DependencyInjection;
using SunEngine.Commons.Cache.CacheModels;
using SunEngine.Commons.DataBase;

namespace SunEngine.Commons.Cache.CachePolicy
{
    public class CustomCachePolicy : ICachePolicy

    {
        private IServiceCollection services;

        public bool CanCache(CategoryCached category, int? page = null)
        {
            throw new System.NotImplementedException();
        }
    }
}