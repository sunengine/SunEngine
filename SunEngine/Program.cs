using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SunEngine.DataSeed;
using SunEngine.Migrations;
using SunEngine.Start;

namespace SunEngine
{
    public class Program
    {
        private static StartupConfiguration StartupConfiguration;

        private const string HelpCommand = "help";
        private const string ServerCommand = "server";
        private const string VersionCommand = "version";
        private const string MigrateCommand = "migrate";
        private const string InitCommand = "init";
        private const string SeedCommand = MainSeeder.SeedCommand;
        private const string AppendCategoriesNamesCommand = "append-cat-name";
        
        public static void Main(string[] args)
        {
            StartupConfiguration = new StartupConfiguration(args);

            if (args.Any(x => x == HelpCommand))
                InfoPrinter.PrintHelp();

            else if (args.Any(x => x == ServerCommand))
                RunServer(args);

            else if (args.Any(x => x == VersionCommand))
                InfoPrinter.PrintVersion();

            else if (args.Any(x => x == MigrateCommand || x == InitCommand|| x == SeedCommand))
            {
                if (args.Any(x => x == MigrateCommand))
                    new MainMigrator(StartupConfiguration.ConfigurationDirectoryRoute).Migrate();

                if (args.Any(x => x == InitCommand))
                    new MainSeeder(StartupConfiguration.ConfigurationDirectoryRoute).SeedInitialize();

                if (args.Any(x => x.StartsWith(SeedCommand)))
                    new MainSeeder(StartupConfiguration.ConfigurationDirectoryRoute)
                        .SeedAddTestData(args.Where(x => x.StartsWith(SeedCommand)).ToList(), args.Any(x => x == AppendCategoriesNamesCommand));
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