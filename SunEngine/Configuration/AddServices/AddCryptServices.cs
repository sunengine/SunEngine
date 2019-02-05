using Microsoft.Extensions.DependencyInjection;
using SunEngine.Admin.Services;
using SunEngine.Commons.Services;
using SunEngine.Managers;
using SunEngine.Presenters;
using SunEngine.Security;
using SunEngine.Security.Authorization;
using SunEngine.Services;

namespace SunEngine.Configuration
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