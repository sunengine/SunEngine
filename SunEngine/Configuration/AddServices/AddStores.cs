using Microsoft.Extensions.DependencyInjection;
using SunEngine.DataBase;
using SunEngine.Stores;

namespace SunEngine.Configuration.AddServices
{
    public static class AddStoresExtensions
    {
        public static void AddStores(this IServiceCollection services, DataBaseFactory dataBaseFactory)
        {
            // Add Singleton Stores
            var userGroupStore = new UserGroupStore(dataBaseFactory);

            services.AddSingleton<IUserGroupStore>(userGroupStore);

            services.AddSingleton<ICategoriesStore, CategoriesStore>();

            services.AddSingleton<SpamProtectionStore>();
        }
    }
}