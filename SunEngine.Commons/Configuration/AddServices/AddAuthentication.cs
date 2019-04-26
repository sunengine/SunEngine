using Microsoft.Extensions.DependencyInjection;
using SunEngine.Commons.Security;

namespace SunEngine.Commons.Configuration.AddServices
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
                .AddScheme<SunJwtOptions, SunJwtHandler>(SunJwt.Scheme, SunJwt.Scheme, options => { });
        }
    }
}