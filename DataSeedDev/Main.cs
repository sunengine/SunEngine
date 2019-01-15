using System;
using System.IO;
using System.Linq;
using SunEngine.Commons.DataBase;
using SunEngine.Seeder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            SeedDataBase();
        }

        static void SeedDataBase()
        {
            var configurationBuilder = new ConfigurationBuilder();
                
            if (File.Exists(Path.GetFullPath("../SunEngine/MyDataBaseConnection.json")))
            {
                configurationBuilder.AddJsonFile(Path.GetFullPath("../SunEngine/MyDataBaseConnection.json"), optional: false, reloadOnChange: true);
            }
            else
            {
                configurationBuilder.AddJsonFile(Path.GetFullPath("../SunEngine/DataBaseConnection.json"), optional: false, reloadOnChange: true);
            }

            var configuration = configurationBuilder.Build();


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