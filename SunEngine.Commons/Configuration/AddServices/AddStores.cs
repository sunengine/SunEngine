using System;
using AngleSharp.Css;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SunEngine.Commons.Cache.CachePolicy;
using SunEngine.Commons.Configuration.Options;
using SunEngine.Commons.Cache.Services;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Utils.CustomExceptions;

namespace SunEngine.Commons.Configuration.AddServices
{
    public static class AddStoresExtensions
    {
        public static void AddStores(this IServiceCollection services, DataBaseFactory dataBaseFactory)
        {
            // Add Singleton Stores
            var userGroupStore = new RolesCache(dataBaseFactory);

            services.AddSingleton<IRolesCache>(userGroupStore);

            services.AddSingleton<ICategoriesCache, CategoriesCache>();

            services.AddSingleton<IContentCache, CategoryContentCache>();

            services.AddSingleton<CacheKeyGenerator>();

            services.AddSingleton<SpamProtectionCache>();
        }

        // Temporary solution
        public static void AddCachePolicy(this IServiceCollection services) => services.AddScoped(GetCachePolicy);

        private static ICachePolicy GetCachePolicy(IServiceProvider provider)
        {
            var cacheOptions = provider.GetRequiredService<IOptions<CacheOptions>>();
            if(cacheOptions == null)
                throw new NotFoundServiceException("Cache policy must be added after loading settings from database");

            switch (cacheOptions.Value.CurrentCachePolicy)
            {
                case CachePolicy.AlwaysPolicy:
                    return new AlwaysCachePolicy();
                case CachePolicy.NeverPolicy:
                    return new NeverCachePolicy();
                case CachePolicy.CustomPolicy:
                    return new CustomCachePolicy();
                default:
                    throw new InvalidOperationException("No operation is defined for this cache policy");
            }
        }
    }
}