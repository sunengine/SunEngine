using System;
using System.IO;
using SunEngine.DataSeed;
using SunEngine.Migrations;

namespace SunEngine.Cli
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StartupConfiguration config = new StartupConfiguration(args);
            ExitApplicationIfConfigurationIsNotValid(config);
            
            if (config.PrintHelp)
                InfoPrinter.PrintHelp();

            else if (config.PrintVersion)
                InfoPrinter.PrintVersion();

            else if (config.CheckDatabaseAvailability)
                new MainSeeder(config.ConfigRootDir).СheckConnection();

            else if (ShouldUpdateData(config))
            {
                if (config.Migrate)
                    new MainMigrator(config.ConfigRootDir).Migrate();

                if (config.InitializeCoreData)
                    new MainSeeder(config.ConfigRootDir).SeedInitialize();

                if (config.SeedWithTestData)
                    new MainSeeder(config.ConfigRootDir).SeedAddTestData(config.CategoryTokensToSeed,
                        config.SeedWithCategoryNames);
            }

            else if (config.StartServer)
                new ServerStartup().RunServer(config);
            else
                InfoPrinter.PrintStartWithNoArgumentsInfo();
            
        }

        private static bool ShouldUpdateData(StartupConfiguration startupConfiguration)
        {
            return startupConfiguration.Migrate ||
                   startupConfiguration.InitializeCoreData ||
                   startupConfiguration.SeedWithTestData;
        }

        private static void ExitApplicationIfConfigurationIsNotValid(StartupConfiguration startupConfiguration)
        {
            bool failed = !TestIfConfigurationDirectoryExists(startupConfiguration.ConfigRootDir);

            if (failed)
                Environment.Exit(0);


            bool TestIfConfigurationDirectoryExists(string dirPath)
            {
                if (Directory.Exists(dirPath))
                    return true;

                Console.WriteLine($"Configuration directory \"{dirPath}\" does not exists.");
                return false;
            }
        }
    }
}
