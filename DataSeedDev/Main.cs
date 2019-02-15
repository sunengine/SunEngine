using DataSeedDev.Seeder;
using Microsoft.Extensions.Configuration;
using SunEngine.DataBase;
using SunEngine.Utils;

namespace DataSeedDev
{
    class Program
    {
        static void Main(string[] args)
        {
            SeedDataBase();
        }

        static void SeedDataBase()
        {
            string dbSettingsFile = SettingsFileLocator.GetSettingFilePath("DataBaseConnection.json");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(dbSettingsFile, optional: false, reloadOnChange: true)
                .Build();


            var dataBaseConfiguration = configuration.GetSection("DataBaseConnection");
            var providerName = dataBaseConfiguration["Provider"];
            var connectionString = dataBaseConfiguration["ConnectionString"];

            using (DataBaseConnection db = new DataBaseConnection(providerName, connectionString))
            {
                var dc = new LocalSeeder().Seed();
                new DataBaseSeeder(db, dc).Seed().PostSeed();
            }
        }

        
    }
}