using System.IO;
using System.Linq;
using System.Reflection;
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
            configDir = args.FirstOrDefault(x => x.StartsWith("config:"));
            if (configDir != null)
                configDir = configDir.Substring("config:".Length);
            else
                configDir = "Config";

            configDir = Path.GetFullPath(configDir);

            if (args.Length == 0 || args.Any(x => x == "help"))
            {
                InfoPrinter.PrintHelp();
            }
            else if (args.Any(x => x == "server"))
            {
                RunServer(args);
            }
            else if (args.Any(x => x == "version"))
            {
                InfoPrinter.PrintVersion();
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

                if (args.Any(x => x.StartsWith("seed")))
                {
                    MainSeeder ms = new MainSeeder(configDir);

                    var catsTokens = args.Where(x => x.StartsWith("seed")).ToList();

                    if (catsTokens.Contains("seed"))
                        catsTokens[catsTokens.IndexOf("seed")] = "seed:Root";

                    var tokensCleared = catsTokens.Select(x => x.Substring("seed:".Length));

                    bool titleAppendCategoryName = args.Any(x => x == "append-cat-name");

                    ms.SeedAddTestData(tokensCleared, titleAppendCategoryName);
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
                    string dbSettingFile = Path.GetFullPath(Path.Combine(configDir, "DataBaseConnection.json"));
                    string mainSettingsFile = Path.GetFullPath(Path.Combine(configDir, "SunEngine.json"));
                    string logSettingsFile = Path.GetFullPath(Path.Combine(configDir, "LogConfig.json"));

                    config.AddJsonFile(logSettingsFile, optional: false, reloadOnChange: false);
                    config.AddJsonFile(dbSettingFile, optional: false, reloadOnChange: false);
                    config.AddJsonFile(mainSettingsFile, optional: false, reloadOnChange: false);
                    config.AddCommandLine(args);
                });
    }
}