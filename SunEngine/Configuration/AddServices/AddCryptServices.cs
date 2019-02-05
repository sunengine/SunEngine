using Microsoft.Extensions.DependencyInjection;
using SunEngine.Services;

namespace SunEngine.Configuration.AddServices
{
    internal static class AddCryptServicesExtensions
    {
        public static void AddCryptServices(this IServiceCollection services)
        {
            CryptService cryptService = new CryptService();
            cryptService.AddCryptorKey(CaptchaService.CryptserviceName);
            services.AddSingleton(cryptService);
        }
    }
}