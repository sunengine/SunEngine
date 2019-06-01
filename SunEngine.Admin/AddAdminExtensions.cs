using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Presenters;
using SunEngine.Admin.Services;
using SunEngine.Core.Configuration.Options;

namespace SunEngine.Admin
{
    public static class AddAdminExtensions
    {
        public static void AddAdmin(this IServiceCollection services)
        {
            services.AddScoped<CacheSettingsManager>();
            services.AddScoped<CategoriesAdminManager>();
            services.AddScoped<RolesPermissionsAdminService>();
            services.AddScoped<IUserRolesAdminPresenter, UserRolesAdminPresenter>();
            services.AddScoped<ICategoriesAdminPresenter, CategoriesAdminPresenter>();
            services.AddScoped<CacheSettingsPresentor>();
            services.AddScoped<IUserRolesAdminPresenter, UserRolesAdminPresenter>();
            services.AddScoped<IMenuAdminPresenter, MenuAdminPresenter>();
            services.AddScoped<IMenuAdminManager, MenuAdminManager>();            
            services.AddScoped<ImageCleanerAdminService>();
        }
    }
}