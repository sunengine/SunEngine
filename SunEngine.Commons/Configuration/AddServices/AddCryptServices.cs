using Microsoft.Extensions.DependencyInjection;
using SunEngine.Commons.Security.Captcha;
using SunEngine.Commons.Security.Cryptography;

namespace SunEngine.Commons.Configuration.AddServices
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