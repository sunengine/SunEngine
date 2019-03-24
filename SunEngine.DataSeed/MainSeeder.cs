using System.Linq;
using Microsoft.Extensions.Configuration;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Utils;
using SunEngine.DataSeed.Seeder;

namespace SunEngine.DataSeed
{
    public class MainSeeder
    {
        private readonly string providerName;
        private readonly string connectionString;
        private readonly string configDirPath;

        public MainSeeder(string configDirPath = "Config")
        {
            string dbSettingsFile = SettingsFileLocator.GetSettingFilePath(configDirPath, "DataBaseConnection.json");
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(dbSettingsFile, optional: false, reloadOnChange: true)
                .Build();

            var dataBaseConfiguration = configuration.GetSection("DataBaseConnection");
            providerName = dataBaseConfiguration["Linq2dbProvider"];
            connectionString = dataBaseConfiguration["ConnectionString"];
        }

        public void SeedInitialize()
        {
            using (DataBaseConnection db = new DataBaseConnection(providerName, connectionString))
            {
                DataContainer dataContainer = new InitialSeeder(configDirPath).Seed();
                new DataBaseSeeder(db, dataContainer).SeedInitial();
            }
        }

        public void SeedAddTestData(string settings = "")
        {
            using (DataBaseConnection db = new DataBaseConnection(providerName, connectionString))
            {
                DataContainer dataContainer = new DataContainer
                {
                    Categories = db.Categories.ToList()
                };
                new MaterialsSeeder(dataContainer).Seed();
                new DataBaseSeeder(db, dataContainer).SeedMaterials().PostSeedMaterials();
            }
        }
    }
}