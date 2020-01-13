using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.DataBase;
using SunEngine.Core.Services;

namespace SunEngine.Core.Configuration.AddServices
{
	public static class AddCiphersExtensions
	{
		public static void AddCiphers(this IServiceCollection services, IDataBaseFactory dbFactory)
		{
			CryptService cryptService = new CryptService(dbFactory);
			cryptService.AddCipherKey(CaptchaService.CypherName);
			services.AddSingleton<ICryptService>(cryptService);
		}
	}
}