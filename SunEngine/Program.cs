using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Migrations;
using SunEngine.DataSeed;

namespace SunEngine
{
    public class Program
    {
        private static string configDir;
        
        public static void Main(string[] args)
        {
            configDir = args.FirstOrDefault(x => x.StartsWith("c:"));
            if (configDir != null)
                configDir = configDir.Substring(2);
            else
                configDir = "Config";

            configDir = Path.GetFullPath(configDir);


            if (args.Any(x => x == "server"))
            {
                RunServer(args);
            }
            else
            {
                if (args.Any(x => x == "migrate"))
                {
                    MainMigrator mainMigrator = new MainMigrator(configDir);
                    mainMigrator.Migrate();
                }

                if (args.Any(x => x == "init"))
                {
                    MainSeeder ms = new MainSeeder(configDir);
                    ms.SeedInitialize();
                }

                if (args.Any(x => x == "add-test-data"))
                {
                    MainSeeder ms = new MainSeeder(configDir);
                    ms.SeedAddTestData();
                }
            }
        }

        public static void RunServer(string[] args)
        {
            var webHost = CreateWebHostBuilder(args).Build();

            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseKestrel()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    string dbSettingFile = Path.GetFullPath(Path.Combine(configDir,"DataBaseConnection.json"));
                    string mainSettingsFile = Path.GetFullPath(Path.Combine(configDir,"SunEngine.json"));
                    string logSettingsFile = Path.GetFullPath(Path.Combine(configDir,"LogConfig.json"));

                    config.AddJsonFile(logSettingsFile, optional: false, reloadOnChange: false);
                    config.AddJsonFile(dbSettingFile, optional: false, reloadOnChange: false);
                    config.AddJsonFile(mainSettingsFile, optional: false, reloadOnChange: false);
                    config.AddCommandLine(args);
                });
    }
}