using System;
using System.IO;


namespace SunEngine.Cli
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            StartupConfiguration config = new StartupConfiguration(args);

            if (config.PrintHelp || config.PrintVersion)
            {
                if (config.PrintHelp)
                    InfoPrinter.PrintHelp();

                else if (config.PrintVersion)
                    InfoPrinter.PrintVersion();

                return;
            }

            UseConfigurationDirectory(config);

            if (config.CheckDatabaseAvailability)
            {
                if (!DataSeed().PrintDbConnectionAvailability())
                    Environment.Exit(1);
            }

            else if (ShouldUpdate(config))
            {
                if (config.Migrate)
                    Migrations().Migrate();

                if (config.InitializeCoreData)
                    DataSeed().SeedInitialize();

                if (config.SeedWithTestData)
                    DataSeed().SeedAddTestData(config.CategoryTokensToSeed, config.SeedWithCategoryNames);
            }

            else if (config.StartServer)
                new ServerStartup().RunServer(config);
            else
                InfoPrinter.PrintNoArgumentsInfo();


            Migrations.Main Migrations() => new Migrations.Main(config.ConfigRootDir);

            DataSeed.Main DataSeed() => new DataSeed.Main(config.ConfigRootDir);
        }

        private static bool ShouldUpdate(StartupConfiguration startupConfiguration)
        {
            return startupConfiguration.Migrate ||
                   startupConfiguration.InitializeCoreData ||
                   startupConfiguration.SeedWithTestData;
        }

        /// <summary>
        /// Detect configuration directory and exit if it is not exists
        /// </summary>
        private static void UseConfigurationDirectory(StartupConfiguration startupConfiguration)
        {
            startupConfiguration.InitConfigurationDirectory();
            
            bool failed = !TestIfConfigurationDirectoryExists(startupConfiguration.ConfigRootDir);

            if (failed)
                Environment.Exit(1);


            bool TestIfConfigurationDirectoryExists(string dirPath)
            {
                if (Directory.Exists(dirPath))
                    return true;

                Console.WriteLine($@"Configuration directory ""{dirPath}"" does not exists.");
                return false;
            }
        }
    }
}
