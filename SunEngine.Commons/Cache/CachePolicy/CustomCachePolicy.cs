using Microsoft.Extensions.DependencyInjection;
using SunEngine.Commons.Cache.CacheModels;
using SunEngine.Commons.DataBase;

namespace SunEngine.Commons.Cache.CachePolicy
{
    public class CustomCachePolicy : ICachePolicy
    {
        public bool CanCache(CategoryCached category, int? page = null)
        {
            if (category.Parent?.CacheSettings == null)
                return false;

            var cachingPageCount = category.Parent.CacheSettings.PagesAmount;
            if (cachingPageCount != 0 && page >= cachingPageCount)
                return false;
            
            return true;
        }
    }
}