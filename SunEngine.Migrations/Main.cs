using System;
using System.IO;
using System.Net.Sockets;
using FluentMigrator.Runner;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Errors;
using SunEngine.Migrations.Migrations;


namespace SunEngine.Migrations
{
    /// <summary>
    /// Class to migrate database or create initial structure with FluentMigrator Framework
    /// </summary>
    public class Main
    {
        private const string DataBaseConnectionFileName = "DataBaseConnection.json";

        private readonly string DataBaseConnectionFilePath;

        
        public Main(string configDirPath)
        {
            DataBaseConnectionFilePath = Path.GetFullPath(Path.Combine(configDirPath, DataBaseConnectionFileName));
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
                .AddJsonFile(DataBaseConnectionFilePath, false, false)
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
                throw new SunDataBaseException("The connection could not be made. " +
                                                         "Check the database you are trying to connect exists.", e);
            }
        }
    }
}
