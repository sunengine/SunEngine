using Microsoft.Extensions.DependencyInjection;
using SunEngine.Security.Authentication;

namespace SunEngine.Configuration.AddServices
{
    public static class AddAuthenticationExtensions
    {
        public static void AddAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = MyJwt.Scheme;
                    options.DefaultChallengeScheme = MyJwt.Scheme;
                })
                .AddScheme<MyJwtOptions, MyJwtHandler>(MyJwt.Scheme, MyJwt.Scheme, options => { });
        }
    }
}