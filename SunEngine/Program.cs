using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SunEngine.Configuration;

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
                    string settingsFile = GetSettingFilePath(env);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: false)
                        .AddJsonFile(settingsFile, optional: false, reloadOnChange: false);
                });

        // ProjectFolder Local, SettingsFolder Local,  ProjectFolder, SettingsFolder
        private static string GetSettingFilePath(IHostingEnvironment env)
        {
            string fileName = "Local.SunEngine.json";
            string[] dirs =  {"","../","Settings"};

            foreach (var dir in dirs)
            {
                string path = Path.Combine(env.ContentRootPath,dir,fileName);
                if (File.Exists(path))
                    return path;
            }
            
            fileName = "SunEngine.json";
            foreach (var dir in dirs)
            {
                string path = Path.Combine(env.ContentRootPath,dir,fileName);
                if (File.Exists(path))
                    return path;
            }
            
            throw new Exception("Can not locate Local.SunEngine.json or SunEngine.json");
        }
    }
}