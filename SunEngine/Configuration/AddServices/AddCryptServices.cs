using Microsoft.Extensions.DependencyInjection;
using SunEngine.Security;
using SunEngine.Security.Captcha;
using SunEngine.Security.Cryptography;
using SunEngine.Services;

namespace SunEngine.Configuration.AddServices
{
    public static class AddCryptServicesExtensions
    {
        public static void AddCryptServices(this IServiceCollection services)
        {
            CryptService cryptService = new CryptService();
            cryptService.AddCryptorKey(CaptchaService.CryptserviceName);
            services.AddSingleton(cryptService);
        }
    }
}