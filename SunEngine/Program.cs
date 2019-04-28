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
        public const string HelpCommand = "help";
        public const string ServerCommand = "server";
        public const string VersionCommand = "version";
        public const string MigrateCommand = "migrate";
        public const string InitCommand = "init";
        public const string SeedCommand = MainSeeder.SeedCommand;
        public const string AppendCategoriesNamesCommand = "append-cat-name";
        public const string ConfigCommand = "config:";

        
        private static string configDir;
        
        

        public static void Main(string[] args)
        {
            configDir = args.FirstOrDefault(x => x.StartsWith(ConfigCommand));
            configDir = configDir != null ? configDir.Substring(ConfigCommand.Length) : "Config";

            configDir = Path.GetFullPath(configDir);


            if (args.Any(x => x == HelpCommand))
                InfoPrinter.PrintHelp();

            else if (args.Any(x => x == ServerCommand))
                RunServer(args);

            else if (args.Any(x => x == VersionCommand))
                InfoPrinter.PrintVersion();

            else if (args.Any(x => x == MigrateCommand || x == InitCommand|| x == SeedCommand))
            {
                if (args.Any(x => x == MigrateCommand))
                    new MainMigrator(configDir).Migrate();

                if (args.Any(x => x == InitCommand))
                    new MainSeeder(configDir).SeedInitialize();

                if (args.Any(x => x.StartsWith(SeedCommand)))
                    new MainSeeder(configDir).SeedAddTestData(args.Where(x => x.StartsWith(SeedCommand)).ToList(), args.Any(x => x == AppendCategoriesNamesCommand));
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
    }
}