using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SunEngine.Configuration;
using SunEngine.Utils;

namespace SunEngine
{
    public class Program
    {
        public static void Main(string[] args)
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
                    string mainSettingsFile = SettingsFileLocator.GetSettingFilePath("app.settings.json");
                    string logSettingsFile = SettingsFileLocator.GetSettingFilePath( "log.settings.json");
                    string logSettingsFileEnv =
                        SettingsFileLocator.GetSettingFilePath( $"log.settings.{env.EnvironmentName.ToLower()}.json", true);

                    config.AddJsonFile(logSettingsFile, optional: false, reloadOnChange: false)
                        .AddJsonFile(logSettingsFileEnv, optional: true, reloadOnChange: false)
                        .AddJsonFile(mainSettingsFile, optional: false, reloadOnChange: false);
                });

      
    }
}