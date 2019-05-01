using SunEngine.DataSeed;
using SunEngine.Migrations;


namespace SunEngine
{
    public class Program
    {
        private static readonly InfoPrinter InfoPrinter = new InfoPrinter();
        private static readonly ServerStartup ServerStartup = new ServerStartup();

        public static void Main(string[] args)
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(args);

            if (startupConfiguration.PrintHelp)
                InfoPrinter.PrintHelp();

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
                ServerStartup.RunServer(startupConfiguration);    
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
