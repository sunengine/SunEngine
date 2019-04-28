using System.IO;
using System.Linq;
using SunEngine.DataSeed;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SunEngine.Migrations;


namespace SunEngine
{
    public class Program
    {
        private static StartupConfiguration StartupConfiguration;

        private const string MigrateCommand = "migrate";
        private const string InitCommand = "init";
        private const string SeedCommand = MainSeeder.SeedCommand;
        
        public static void Main(string[] args)
        {
            StartupConfiguration = new StartupConfiguration(args);

            if (StartupConfiguration.PrintHelp)
                InfoPrinter.PrintHelp();

            else if (StartupConfiguration.StartServer)
                RunServer(args);

            else if (StartupConfiguration.PrintVersion)
                InfoPrinter.PrintVersion();

            else if (args.Any(x => x == MigrateCommand || x == InitCommand || x == SeedCommand))
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
                if (SunEngineDllRunServer(args))
                    RunServer(args);
                else
                    InfoPrinter.PrintVoidStartInfo();
            }
        }

        public static void RunServer(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();

            IHostingEnvironment env = (IHostingEnvironment) webHost.Services.GetService(typeof(IHostingEnvironment));
            IConfiguration conf = (IConfiguration) webHost.Services.GetService(typeof(IConfiguration));

            Startup.SetExceptionsMode(env, conf);

            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    string dbSettingFile = Path.GetFullPath(Path.Combine(StartupConfiguration.ConfigurationDirectoryRoute, "DataBaseConnection.json"));
                    string mainSettingsFile = Path.GetFullPath(Path.Combine(StartupConfiguration.ConfigurationDirectoryRoute, "SunEngine.json"));
                    string logSettingsFile = Path.GetFullPath(Path.Combine(StartupConfiguration.ConfigurationDirectoryRoute, "LogConfig.json"));

                    config.AddJsonFile(logSettingsFile, false, false);
                    config.AddJsonFile(dbSettingFile, false, false);
                    config.AddJsonFile(mainSettingsFile, false, false);
                    config.AddCommandLine(args);
                });

        static bool SunEngineDllRunServer(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();

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