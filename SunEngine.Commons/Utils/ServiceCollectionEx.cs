using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Commons.Utils.CustomExceptions;

namespace SunEngine.Commons.Utils
{
    public static class ServiceCollectionEx
    {
        public static IServiceCollection Replace<TService, TImplementation>(this IServiceCollection services,
             ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TService : class
            where TImplementation : class, TService
        {
            var deletingService = services.FirstOrDefault(d => d.ServiceType == typeof(TService))
                                  ?? throw new NotFoundServiceException(
                                      $"Not found service {typeof(TService).FullName} in container.");
            services.Remove(deletingService);
            
            var replacingService = new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime);
            services.Add(replacingService);

            return services;
        }
    }
}