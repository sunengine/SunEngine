using Microsoft.Extensions.DependencyInjection;
using SunEngine.Managers;

namespace SunEngine.Configuration.AddServices
{
    static internal class AddManagersExtensions
    {
        public static void AddManagers(this IServiceCollection services)
        {
            services.AddScoped<MaterialsManager>();  
            services.AddScoped<MessagesManager>();  
            services.AddScoped<PersonalManager>();
            services.AddScoped<ProfileManager>();
            services.AddScoped<TagsManager>();
        }
    }
}