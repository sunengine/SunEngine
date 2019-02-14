using Microsoft.Extensions.DependencyInjection;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Presenters;
using SunEngine.Admin.Services;

namespace SunEngine.Configuration.AddServices
{
    public static class AddAdminServicesExtensions
    {
        public static void AddAdminServices(this IServiceCollection services)
        {
            services.AddScoped<CategoriesManager>();
            services.AddScoped<RolesAdminService>();
            services.AddScoped<IUserRolesPresenter, UserRolesPresenter>();
            services.AddScoped<ICategoriesAdminPresenter, CategoriesAdminPresenter>();
            services.AddScoped<IUserRolesPresenter, UserRolesPresenter>();

        }
    }
}