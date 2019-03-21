using System;
using System.IO;
using DbTool.Migrations;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Utils;

namespace DbTool
{
    public class Migrator
    {
        private string configDirName;
        private const string configFileName = "DataBaseConnection.json";
        
        public Migrator(string configDirPath)
        {
            this.configDirPath = configDirPath;
        }
        
        public void Migrate()
        {
            var serviceProvider = CreateServices();

            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
        }


        /// <summary>
        /// Configure the dependency injection services
        /// </sumamry>
        private IServiceProvider CreateServices()
        {
SettingsFileLocator.GetSettingFilePath("")
            string dbSettingsFile = GetSettingFilePath(configFile);
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(dbSettingsFile, optional: false, reloadOnChange: true)
                .Build();


            var dataBaseConfiguration = configuration.GetSection("DataBaseConnection");
            var providerName = dataBaseConfiguration["Provider"];
            DbProvider.Initialize(providerName);
            var connectionString = dataBaseConfiguration["ConnectionString"];


            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Select DataBaseSupport
                    //.AddMySql5() 
                    //.AddPostgres()
                    .AddSQLite()
                    .WithGlobalConnectionString(connectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(Initial).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        /// <summary>
        /// Update the database
        /// </sumamry>
        private void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
            
            runner.MigrateUp();
        }

       
    }

    
    public static class DbProvider
    {       
        public static string Name { get; private set; }

        public static bool IsPostgre { get; private set; }

        public static void Initialize(string name)
        {
            Name = name;

            if (name.StartsWith("Postgre"))
            {
                IsPostgre = true;
            }
        }
    }
}