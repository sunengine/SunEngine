using Microsoft.Extensions.DependencyInjection;
using SunEngine.Security.Authentication;
using SunEngine.Security.Authorization;

namespace SunEngine.Configuration.AddServices
{
    public static class AddAuthorizationExtensions
    {
        public static void AddAuthorization(this IServiceCollection services)
        {
            services.AddSingleton<OperationKeysContainer>();
            services.AddSingleton<IAuthorizationService, AuthorizationService>();
        
            services.AddScoped<MaterialsAuthorization>();
            services.AddScoped<MessageAuthorization>();
        
            services.AddScoped<JwtService>();
            
            services.AddSingleton<JwtBlackListService>();
        }
    }
}