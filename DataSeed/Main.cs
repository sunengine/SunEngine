using System.IO;
using DataSeed.Seeder;
using Microsoft.Extensions.Configuration;
using SunEngine.DataBase;
using SunEngine.Utils;

namespace DataSeed
{
    class Program
    {
        static void Main(string[] args)
        {
            SeedDataBase(args.Length == 0 ? "SeedConfig" : args[0]);
        }

        static void SeedDataBase(string configDir)
        {
            string dbSettingsFile = SettingsFileLocator.GetSettingFilePath(configDir,"DataBaseConnection.json");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(dbSettingsFile, optional: false, reloadOnChange: true)
                .Build();


            var dataBaseConfiguration = configuration.GetSection("DataBaseConnection");
            var providerName = dataBaseConfiguration["Provider"];
            var connectionString = dataBaseConfiguration["ConnectionString"];

            using (DataBaseConnection db = new DataBaseConnection(providerName, connectionString))
            {
                DataContainer dataContainer = new InMemorySeeder(configDir).Seed();
                new DataBaseSeeder(db, dataContainer).Seed().PostSeed();
            }
        }

        
    }
}