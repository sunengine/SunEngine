using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SunEngine.DataSeed;
using SunEngine.Migrations;

namespace SunEngine
{
    public class Program
    {
        public static string configDir;
        private const string ConfigurationArgumentName = "config:"; 
        private const string DefaultConfigurationFileName = "Config";

        private const string HelpCommand = "help";
        private const string ServerCommand = "server";
        private const string VersionCommand = "version";
        private const string MigrateCommand = "migrate";
        private const string InitCommand = "init";
        private const string SeedCommand = MainSeeder.SeedCommand;
        private const string AppendCategoriesNamesCommand = "append-cat-name";
        
        public static void SetUpConfigurationDirectory(IEnumerable<string> arguments)
        {
            var configurationDirectory = GetConfigurationDirectory(arguments);
            configDir = Path.GetFullPath(configurationDirectory);   
        }

        private static string GetConfigurationDirectory(IEnumerable<string> arguments)
        {
            var configurationProperty = arguments.FirstOrDefault(x => x.StartsWith(ConfigurationArgumentName));
            if (string.IsNullOrEmpty(configurationProperty))
            {
                Console.Write("Property for configuration wasn't set. Default configuration will be used.");
                return DefaultConfigurationFileName;
            }
            
            var configurationFileName = configurationProperty.Substring(ConfigurationArgumentName.Length).Trim();
            if (string.IsNullOrEmpty(configurationFileName))
            {
                Console.Write("Property for configuration was empty or blank. Default configuration will be used.");
                return DefaultConfigurationFileName;
            }

            Console.Write($"Configuration file {configurationFileName} will be used.");
            return configurationFileName;
        }
        
        public static void Main(string[] args)
        {
            SetUpConfigurationDirectory(args);

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