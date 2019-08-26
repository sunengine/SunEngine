using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Security;

namespace SunEngine.Core.Configuration.AddServices
{
    public static class AddAuthenticationExtensions
    {
        public static void AddAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = SunJwt.Scheme;
                    options.DefaultChallengeScheme = SunJwt.Scheme;
                })
                .AddScheme<SunJwtOptions, SunJweHandler>(SunJwt.Scheme, SunJwt.Scheme, options => { });
        }
    }
}