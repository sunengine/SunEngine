using Microsoft.Extensions.DependencyInjection;
using SunEngine.Commons.Services;
using SunEngine.Security;
using SunEngine.Security.Authorization;

namespace SunEngine.Configuration.AddServices
{
    static internal class AddAuthorizationExtensions
    {
        public static void AddAuthorization(this IServiceCollection services)
        {
            services.AddSingleton<OperationKeysContainer>();
            services.AddSingleton<IAuthorizationService, AuthorizationService>();
        
            services.AddScoped<CategoriesAuthorization>();
            services.AddScoped<MaterialsAuthorization>();
            services.AddScoped<MessageAuthorization>();
        
            services.AddScoped<AuthService>();
        }
    }
}