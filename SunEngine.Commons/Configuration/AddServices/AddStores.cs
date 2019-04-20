using Microsoft.Extensions.DependencyInjection;
using SunEngine.Commons.Cache;
using SunEngine.Commons.Cache.Services;
using SunEngine.Commons.DataBase;

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
    }
}