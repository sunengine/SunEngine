using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Services;

namespace SunEngine.Core.Configuration.AddServices
{
	public static class AddImagesServicesExtensions
	{
		public static void AddImagesServices(this IServiceCollection services)
		{
			services.AddSingleton<IImagesNamesService, ImagesNamesService>();
			services.AddSingleton<IImagesService, ImagesService>();
		}
	}
}