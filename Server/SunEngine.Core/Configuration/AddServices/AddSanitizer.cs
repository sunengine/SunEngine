using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Services;

namespace SunEngine.Core.Configuration.AddServices
{
	public static class AddSanitizerExtensions
	{
		public static void AddSanitizer(this IServiceCollection services)
		{
			services.AddSingleton<SanitizerService>();
		}
	}
}