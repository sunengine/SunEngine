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
        
        private static StartupConfiguration StartupConfiguration;
        
        public static void Main(string[] args)
        {
            StartupConfiguration = new StartupConfiguration(args);

            if (StartupConfiguration.PrintHelp)
                InfoPrinter.PrintHelp();

            else if (StartupConfiguration.StartServer)
                RunServer(StartupConfiguration);

            else if (StartupConfiguration.PrintVersion)
                InfoPrinter.PrintVersion();

            else if (ShouldUpdateData(StartupConfiguration))
            {
                if (StartupConfiguration.Migrate)
                    new MainMigrator(StartupConfiguration.ConfigurationDirectoryRoute).Migrate();

                if (StartupConfiguration.InitializeCoreData)
                    new MainSeeder(StartupConfiguration.ConfigurationDirectoryRoute).SeedInitialize();

                if (StartupConfiguration.SeedWithTestData)
                    new MainSeeder(StartupConfiguration.ConfigurationDirectoryRoute)
                        .SeedAddTestData(StartupConfiguration.CategoryTokensToSeed,
                            StartupConfiguration.SeedWithCategoryNames);
            }
            else
            {
                if (SunEngineDllRunServer(StartupConfiguration))
                    RunServer(StartupConfiguration);
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
        
        public static void RunServer(StartupConfiguration startupConfiguration)
        {
            var webHost = CreateWebHostBuilder(startupConfiguration).Build();

            IHostingEnvironment env = (IHostingEnvironment) webHost.Services.GetService(typeof(IHostingEnvironment));
            IConfiguration conf = (IConfiguration) webHost.Services.GetService(typeof(IConfiguration));

            Startup.SetExceptionsMode(env, conf);

            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(StartupConfiguration startupConfiguration) =>
            WebHost.CreateDefaultBuilder(startupConfiguration.Arguments)
                .UseKestrel()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    string dbSettingFile = Path.GetFullPath(Path.Combine(startupConfiguration.ConfigurationDirectoryRoute, "DataBaseConnection.json"));
                    string mainSettingsFile = Path.GetFullPath(Path.Combine(startupConfiguration.ConfigurationDirectoryRoute, "SunEngine.json"));
                    string logSettingsFile = Path.GetFullPath(Path.Combine(startupConfiguration.ConfigurationDirectoryRoute, "LogConfig.json"));

                    config.AddJsonFile(logSettingsFile, false, false);
                    config.AddJsonFile(dbSettingFile, false, false);
                    config.AddJsonFile(mainSettingsFile, false, false);
                    config.AddCommandLine(startupConfiguration.Arguments);
                });

        static bool SunEngineDllRunServer(StartupConfiguration startupConfiguration)
        {
            var webHost = CreateWebHostBuilder(startupConfiguration).Build();

            IHostingEnvironment env = (IHostingEnvironment) webHost.Services.GetService(typeof(IHostingEnvironment));
            IConfiguration conf = (IConfiguration) webHost.Services.GetService(typeof(IConfiguration));

            if (bool.TryParse(conf["Dev:SunEngineDllRunServer"], out bool sunEngineDllRunServer))
            {
                if (sunEngineDllRunServer)
                    return true;
            }
            else if (env.IsDevelopment())
            {
                return true;
            }

            return false;
        }
    }
}
