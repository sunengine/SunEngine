using Microsoft.Extensions.DependencyInjection;
using SunEngine.Admin.Services;

namespace SunEngine.Configuration.AddServices
{
    public static class AddAdminServicesExtensions
    {
        public static void AddAdminServices(this IServiceCollection services)
        {
            services.AddScoped<CategoriesAdminService>();
            services.AddScoped<GroupsAdminService>();
            services.AddScoped<GroupsUsersService>();
        }
    }
}