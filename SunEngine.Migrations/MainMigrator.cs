using System;
using System.IO;
using System.Net.Sockets;
using FluentMigrator;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Exceptions.Database.Connection;
using SunEngine.Migrations.Migrations;

namespace SunEngine.Migrations
{
    /// <summary>
    /// Class to migrate database or create initial structure with FluentMigrator Framework
    /// </summary>
    public class MainMigrator
    {
        private string dbConfigFilePath;

        public MainMigrator(string configDirPath)
        {
            dbConfigFilePath = Path.GetFullPath(Path.Combine(configDirPath, "DataBaseConnection.json"));
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
        /// </summary>
        private IServiceProvider CreateServices()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(dbConfigFilePath, optional: false, reloadOnChange: false)
                .Build();

            var dataBaseConfiguration = configuration.GetSection("DataBaseConnection");
            var providerName = dataBaseConfiguration["FluentMigratorProvider"];
            DbProvider.Initialize(providerName);
            var connectionString = dataBaseConfiguration["ConnectionString"];

            return new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb =>
                {
                    rb
                        .AddDb()
                        .WithGlobalConnectionString(connectionString)
                        .ScanIn(typeof(Initial).Assembly).For.Migrations();
                })
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false);
        }

        /// <summary>
        /// Update the database
        /// </summary>
        private static void UpdateDatabase(IServiceProvider serviceProvider)
        {
            var runner = serviceProvider.GetRequiredService<IMigrationRunner>();

            try
            {
                runner.MigrateUp();
            }
            catch (SocketException e)
            {
                throw new SunDatabaseConnectionException("The connection could not be made. " +
                                                         "Check the database you are trying to connect exists.", e);
            }
        }
    }
    
}
