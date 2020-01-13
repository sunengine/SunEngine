using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Security;

namespace SunEngine.Core.Configuration.AddServices
{
	public static class AddSunAuthenticationExtensions
	{
		public static IServiceCollection AddSunAuthentication(this IServiceCollection services)
		{
			services.AddAuthentication(options =>
				{
					options.DefaultAuthenticateScheme = SunJwe.Scheme;
					options.DefaultChallengeScheme = SunJwe.Scheme;
				}
			).AddScheme<SunJweOptions, SunJweHandler>(SunJwe.Scheme, SunJwe.Scheme, options => { });

			return services;
		}
	}
}