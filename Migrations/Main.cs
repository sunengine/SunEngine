using System;
using System.IO;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = CreateServices(args.Length == 0 ? "DataBaseConnection.json" : args[0]);

            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
        }


        /// <summary>
        /// Configure the dependency injection services
        /// </sumamry>
        private static IServiceProvider CreateServices(string configFile)
        {
            configFile = configFile.Trim();

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
                    .AddPostgres()
                    //.AddSQLite()
                    .WithGlobalConnectionString(connectionString)
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(Initial).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        /// <summary>
        /// Update the database
        /// </sumamry>
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            runner.MigrateUp();
        }

        private static string GetSettingFilePath(string fileName)
        {
            string fileLocal = "local." + fileName;
            string pathLocal = Path.GetFullPath(fileLocal);
            if (File.Exists(pathLocal))
                return pathLocal;

            string fileCommon = fileName;
            string pathCommon = Path.GetFullPath(fileCommon);
            if (File.Exists(pathCommon))
                return pathCommon;

            throw new Exception($"Can not locate local.{fileName} or {fileName}");
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