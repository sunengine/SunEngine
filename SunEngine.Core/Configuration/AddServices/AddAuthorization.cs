using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Security;

namespace SunEngine.Core.Configuration.AddServices
{
    public static class AddAuthorizationExtensions
    {
        public static void AddAuthorization(this IServiceCollection services)
        {
            services.AddSingleton<OperationKeysContainer>();
            services.AddSingleton<IAuthorizationService, AuthorizationService>();
        
            services.AddScoped<MaterialsAuthorization>();
            services.AddScoped<CommentsAuthorization>();
        
            services.AddScoped<JweService>();
            
            services.AddSingleton<JweBlackListService>();
        }
    }
}