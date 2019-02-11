using Microsoft.Extensions.DependencyInjection;
using SunEngine.Security;
using SunEngine.Security.Authentication;
using SunEngine.Security.Authorization;

namespace SunEngine.Configuration.AddServices
{
    internal static class AddAuthorizationExtensions
    {
        public static void AddAuthorization(this IServiceCollection services)
        {
            services.AddSingleton<OperationKeysContainer>();
            services.AddSingleton<IAuthorizationService, AuthorizationService>();
        
            services.AddScoped<CategoriesAuthorization>();
            services.AddScoped<MaterialsAuthorization>();
            services.AddScoped<MessageAuthorization>();
        
            services.AddScoped<JwtService>();
            
            services.AddSingleton<JwtBlackListService>();
        }
    }
}