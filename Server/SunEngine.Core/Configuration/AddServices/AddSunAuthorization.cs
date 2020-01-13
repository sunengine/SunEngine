using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Security;

namespace SunEngine.Core.Configuration.AddServices
{
	public static class AddSunAuthorizationExtensions
	{
		public static IServiceCollection AddSunAuthorization(this IServiceCollection services)
		{
			services.AddSingleton<OperationKeysContainer>();
			services.AddSingleton<IAuthorizationService, AuthorizationService>();

			services.AddScoped<MaterialsAuthorization>();
			services.AddScoped<CommentsAuthorization>();

			services.AddScoped<JweService>();

			services.AddSingleton<JweBlackListService>();

			return services;
		}
	}
}