using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Services;
using SunEngine.Core.Utils.TextProcess;

namespace SunEngine.Core.Configuration.AddServices
{
    public static class AddSanitizerExtensions
    {
        public static void AddSanitizer(this IServiceCollection services)
        {
            services.AddSingleton<Sanitizer>();
            services.AddSingleton<SanitizerOptionsService>();
            services.AddSingleton<SanitizerOptions>(serviceProvider =>
            {
                var sanitizerOptionsService = serviceProvider.GetRequiredService<SanitizerOptionsService>();
                return sanitizerOptionsService.GetOptions();
            });
        }
    }
}