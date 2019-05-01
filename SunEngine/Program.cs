using System;
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

        public static void Main(string[] args)
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(args);

            if (startupConfiguration.PrintHelp)
                InfoPrinter.PrintHelp();

            else if (startupConfiguration.StartServer)
                RunServer(startupConfiguration);

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
                if (SunEngineDllRunServer(startupConfiguration))
                    RunServer(startupConfiguration);
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

        private static void RunServer(StartupConfiguration startupConfiguration)
        {
            var webHost = CreateWebHostBuilder(startupConfiguration).Build();

            IHostingEnvironment env = (IHostingEnvironment) webHost.Services.GetService(typeof(IHostingEnvironment));
            IConfiguration conf = (IConfiguration) webHost.Services.GetService(typeof(IConfiguration));
            
            Startup.SetExceptionsMode(env, conf);

            webHost.Run();
        }

        private static IWebHostBuilder CreateWebHostBuilder(StartupConfiguration startupConfiguration) =>
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

        private static bool SunEngineDllRunServer(StartupConfiguration startupConfiguration)
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