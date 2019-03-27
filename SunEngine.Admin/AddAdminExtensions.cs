using Microsoft.Extensions.DependencyInjection;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Presenters;
using SunEngine.Admin.Services;

namespace SunEngine.Admin
{
    public static class AddAdminExtensions
    {
        public static void AddAdmin(this IServiceCollection services)
        {
            services.AddScoped<CategoriesManager>();
            services.AddScoped<RolesPermissionsAdminService>();
            services.AddScoped<IUserRolesPresenter, UserRolesPresenter>();
            services.AddScoped<ICategoriesAdminPresenter, CategoriesAdminPresenter>();
            services.AddScoped<IUserRolesPresenter, UserRolesPresenter>();

        }
    }
}