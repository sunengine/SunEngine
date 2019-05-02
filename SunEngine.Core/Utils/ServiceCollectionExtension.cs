using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace SunEngine.Core.Utils
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection Replace<TService, TImplementation>(
            this IServiceCollection services,
            ServiceLifetime lifetime = ServiceLifetime.Scoped)
            where TService : class
            where TImplementation : class, TService
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));
            services.Remove(descriptorToRemove);
            
            var descriptorToAdd = new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime);
            services.Add(descriptorToAdd);
            return services;
        }
        
        public static IServiceCollection Replace<TService>(
            this IServiceCollection services, object instance)
            where TService : class
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));
            services.Remove(descriptorToRemove);
            
            var descriptorToAdd = new ServiceDescriptor(typeof(TService), instance); // lifetime - singleton
            services.Add(descriptorToAdd);
            return services;
        }
    }
}