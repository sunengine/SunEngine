using Microsoft.Extensions.DependencyInjection;
using SunEngine.Admin.Services;

static internal class AddAdminServicesExtensions
{
    public static void AddAdminServices(this IServiceCollection services)
    {
        services.AddScoped<CategoriesAdminService>();
        services.AddScoped<GroupsAdminService>();
    }
}