using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Services;

namespace SunEngine.Tests
{
    internal static class DefaultInit
    {
        private static string connectionString = "Host=localhost;Database=SunEngine;Username=postgres;Password=password";
        private static string provider = "PostgreSQL.9.5";

        public static DataBaseConnection GetTestDataBaseConnection()
        {
            LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
            return new DataBaseConnection(provider, connectionString);
        }

        public static DataBaseFactory GetTestDataBaseFactory()
        {
            LinqToDB.Common.Configuration.Linq.AllowMultipleQuery = true;
            return new DataBaseFactory(provider, connectionString, new DbMappingSchema());
        }

        public static ServiceProvider DefaultServiceProvider()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddOptions<IOptions<CacheOptions>>();

            CryptService cryptService = new CryptService();
            cryptService.AddCryptorKey(CaptchaService.CryptserviceName);

            serviceCollection.AddSingleton(cryptService);
            return serviceCollection.BuildServiceProvider();
        }
    }
}
