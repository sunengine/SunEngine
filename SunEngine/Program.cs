using System.IO;
using SunEngine.DataSeed;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SunEngine.Migrations;


namespace SunEngine
{
    public class Program
    {
        private static readonly InfoPrinter InfoPrinter = new InfoPrinter();
        private static readonly StartupRunner StartupRunner = new StartupRunner();

        public static void Main(string[] args)
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(args);

            if (startupConfiguration.PrintHelp)
                InfoPrinter.PrintHelp();

            else if (startupConfiguration.StartServer)
                StartupRunner.RunServer(startupConfiguration);

            else if (startupConfiguration.PrintVersion)
                InfoPrinter.PrintVersion();

            else if (ShouldUpdateData(startupConfiguration))
            {
                if (startupConfiguration.Migrate)
                    new MainMigrator(startupConfiguration.ConfigurationDirectoryRoute).Migrate();

                if (startupConfiguration.InitializeCoreData)
                    new MainSeeder(startupConfiguration.ConfigurationDirectoryRoute).SeedInitialize();

                if (startupConfiguration.SeedWithTestData)
                    new MainSeeder(startupConfiguration.ConfigurationDirectoryRoute)
                        .SeedAddTestData(startupConfiguration.CategoryTokensToSeed,
                            startupConfiguration.SeedWithCategoryNames);
            }
            else
            {
                if (StartupRunner.SunEngineDllRunServer(startupConfiguration))
                    StartupRunner.RunServer(startupConfiguration);
                else
                    InfoPrinter.PrintVoidStartInfo();
            }
        }

        private static bool ShouldUpdateData(StartupConfiguration startupConfiguration)
        {
            return startupConfiguration.Migrate ||
                   startupConfiguration.InitializeCoreData ||
                   startupConfiguration.SeedWithTestData;
        }
    }

}
