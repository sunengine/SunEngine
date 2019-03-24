using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SunEngine.Commons.Utils;
using SunEngine.DataSeed;

namespace SunEngine
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string configDir = args.FirstOrDefault(x => x.StartsWith("c:"));
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
                    IHostingEnvironment env = builderContext.HostingEnvironment;
                    string mainSettingsFile = SettingsFileLocator.GetSettingFilePath("SunEngine.json");
                    string logSettingsFile = SettingsFileLocator.GetSettingFilePath("LogConfig.json");
                    string logSettingsFileEnv =
                        SettingsFileLocator.GetSettingFilePath(
                            $"LogConfig.{SettingsFileLocator.GetEnvSuffix(env)}.json",
                            true);

                    config.AddJsonFile(logSettingsFile, optional: false, reloadOnChange: false);
                    if (logSettingsFileEnv != null)
                        config.AddJsonFile(logSettingsFileEnv, optional: true, reloadOnChange: false);

                    config.AddJsonFile(mainSettingsFile, optional: false, reloadOnChange: false);
                    config.AddCommandLine(args);
                });
    }
}