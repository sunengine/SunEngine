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
            SeedDataBase(args?[0] ?? "SeedConfig");
        }

        static void SeedDataBase(string configDir)
        {
            string settingsFilePath = Path.Combine(configDir, "DataBaseConnection.json");
            
            string dbSettingsFile = SettingsFileLocator.GetSettingFilePath(settingsFilePath);
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(dbSettingsFile, optional: false, reloadOnChange: true)
                .Build();


            var dataBaseConfiguration = configuration.GetSection("DataBaseConnection");
            var providerName = dataBaseConfiguration["Provider"];
            var connectionString = dataBaseConfiguration["ConnectionString"];

            using (DataBaseConnection db = new DataBaseConnection(providerName, connectionString))
            {
                var dc = new LocalSeeder(configDir).Seed();
                new DataBaseSeeder(db, dc).Seed().PostSeed();
            }
        }

        
    }
}