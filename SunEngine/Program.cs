using System;
using System.IO;
using System.Linq;
using SunEngine.DataSeed;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SunEngine.Commons.Utils;
using SunEngine.Migrations;


namespace SunEngine
{
    public class Program
    {
        private static string configDir;

        public static void Main(string[] args)
        {
            configDir = args.FirstOrDefault(x => x.StartsWith("config:"));
            configDir = configDir != null ? configDir.Substring("config:".Length) : "Config";

            configDir = Path.GetFullPath(configDir);


            if (args.Any(x => x == "help"))
                InfoPrinter.PrintHelp();

            else if (args.Any(x => x == "server"))
                RunServer(args);

            else if (args.Any(x => x == "version"))
                InfoPrinter.PrintVersion();

            else if(args.Any(x => x == "migrate" || x == "init" || x == "seed"))
            {
                if (args.Any(x => x == "migrate"))
                    new MainMigrator(configDir).Migrate();

                if (args.Any(x => x == "init"))
                    new MainSeeder(configDir).SeedInitialize();

                if (args.Any(x => x.StartsWith("seed")))
                    new MainSeeder(configDir)
                        .SeedAddTestData(
                            args.Where(x => x.StartsWith("seed")).ToList(),
                            args.Any(x => x == "append-cat-name"));

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

            SetExceptionsMode(env, conf);

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

        static void SetExceptionsMode(IHostingEnvironment env, IConfiguration conf)
        {
            void ShowExceptions()
            {
                Console.WriteLine("ShowExceptions mode");
                SunJsonContractResolver.ShowExceptions = true;
            }

            if (bool.TryParse(conf["Dev:ShowExceptions"], out bool showExceptions))
            {
                if (showExceptions)
                    ShowExceptions();
            }
            else if (env.IsDevelopment())
            {
                ShowExceptions();
            }
        }

    }
}