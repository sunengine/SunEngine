using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SunEngine.Core.Cache.CachePolicy;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Utils.CustomExceptions;

namespace SunEngine.Core.Configuration.AddServices
{
    public static class AddStoresExtensions
    {
        public static void AddCaches(this IServiceCollection services, DataBaseFactory dataBaseFactory)
        {
            // Add Singleton cache services
            var rolesCache = new RolesCache(dataBaseFactory);

            services.AddSingleton<IRolesCache>(rolesCache);

            services.AddSingleton<ICategoriesCache, CategoriesCache>();
            
            services.AddSingleton<MenuCache>();


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