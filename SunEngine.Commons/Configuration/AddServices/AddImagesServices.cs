using Microsoft.Extensions.DependencyInjection;
using SunEngine.Commons.Services;

namespace SunEngine.Commons.Configuration.AddServices
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