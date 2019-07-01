using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Services;

namespace SunEngine.Core.Configuration.AddServices
{
    public static class AddSanitizerOptionsExtensions
    {
        public static void AddSanitizerOptions(this IServiceCollection services)
        {
            services.AddSingleton<SanitizerOptionsService>();
            services.AddSingleton<SanitizerOptions>(serviceProvider =>
            {
                var sanitizerOptionsService = serviceProvider.GetRequiredService<SanitizerOptionsService>();
                return sanitizerOptionsService.GetOptions();
            });
        }
    }
}