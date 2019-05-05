using System;
using SunEngine.DataSeed;
using SunEngine.Migrations;

namespace SunEngine.Cli
{
    public class Program
    {
        private static InfoPrinter InfoPrinter = new InfoPrinter();
        private static ServerStartup ServerStartup = new ServerStartup();
        private static MainSeeder MainSeeder;
        private static MainMigrator MainMigrator;

        public static void Main(string[] args)
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(args);

            MainSeeder = new MainSeeder(startupConfiguration.ConfigurationDirectoryRoute);
            MainMigrator = new MainMigrator(startupConfiguration.ConfigurationDirectoryRoute);

            if (startupConfiguration.PrintHelp)
                InfoPrinter.PrintHelp();

            else if (startupConfiguration.PrintVersion)
                InfoPrinter.PrintVersion();

            else if (startupConfiguration.CheckDatabaseAvailability)
                MainSeeder.СheckConnection();

            else if (ShouldUpdateData(startupConfiguration))
            {
                if (startupConfiguration.Migrate)
                    MainMigrator.Migrate();

                if (startupConfiguration.InitializeCoreData)
                    MainSeeder.SeedInitialize();

                if (startupConfiguration.SeedWithTestData)
                    MainSeeder.SeedAddTestData(startupConfiguration.CategoryTokensToSeed,
                        startupConfiguration.SeedWithCategoryNames);
            }

            else if (startupConfiguration.StartServer)
                ServerStartup.RunServer(startupConfiguration);

            else
                Console.WriteLine("Startup arguments wasn't provided. To list available commands use 'help' argument.");
        }

        private static bool ShouldUpdateData(StartupConfiguration startupConfiguration)
        {
            return startupConfiguration.Migrate ||
                   startupConfiguration.InitializeCoreData ||
                   startupConfiguration.SeedWithTestData;
        }
    }
    
}
