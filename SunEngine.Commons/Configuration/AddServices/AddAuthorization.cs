using Microsoft.Extensions.DependencyInjection;
using SunEngine.Commons.Security.Authentication;
using SunEngine.Commons.Security.Authorization;

namespace SunEngine.Commons.Configuration.AddServices
{
    public static class AddAuthorizationExtensions
    {
        public static void AddAuthorization(this IServiceCollection services)
        {
            services.AddSingleton<OperationKeysContainer>();
            services.AddSingleton<IAuthorizationService, AuthorizationService>();
        
            services.AddScoped<MaterialsAuthorization>();
            services.AddScoped<CommentsAuthorization>();
        
            services.AddScoped<JwtService>();
            
            services.AddSingleton<JwtBlackListService>();
        }
    }
}