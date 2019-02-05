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
            var serviceProvider = CreateServices();

            using (var scope = serviceProvider.CreateScope())
            {
                UpdateDatabase(scope.ServiceProvider);
            }
        }

        /// <summary>
        /// Configure the dependency injection services
        /// </sumamry>
        private static IServiceProvider CreateServices()
        {
            string dbSettingsFile = GetDataBaseConnectionFile();
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(dbSettingsFile, optional: false, reloadOnChange: true)
                .Build();
            
            
            var dataBaseConfiguration = configuration.GetSection("DataBaseConnection");
            var providerName = dataBaseConfiguration["Provider"];
            var connectionString = dataBaseConfiguration["ConnectionString"]; 
            
            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Select DataBaseSupport
                    //.AddMySql5() 
                    .AddPostgres()
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
        
        static string GetDataBaseConnectionFile()
        {
            string fileName = "Local.SunEngine.json";
            string[] dirs =  {"","../","../SunEngine/","../SunEngine/Settings"};

            foreach (var dir in dirs)
            {
                string path = Path.GetFullPath(dir + fileName);
                if (File.Exists(path))
                    return path;
            }
            
            fileName = "SunEngine.json";
            foreach (var dir in dirs)
            {
                string path = Path.GetFullPath(dir + fileName);
                if (File.Exists(path))
                    return path;
            }
            
            throw new Exception("Can not locate Local.SunEngine.json or SunEngine.json");
        }
    }

    /*public static class IMigrationRunnerBuilderExtensions
    {
        public static IMigrationRunnerBuilder Add(this IMigrationRunnerBuilder mb,string providerName)
        {
        }
    }*/
}