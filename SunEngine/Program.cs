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
                    string dbSettingsFile = GetDataBaseConnectionFile(env);
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddJsonFile("SunEngine.json", optional: false, reloadOnChange: true)
                        .AddJsonFile(dbSettingsFile, optional: false, reloadOnChange: true);
                });

        private static string GetDataBaseConnectionFile(IHostingEnvironment env)
        {
            string path = Path.Combine(env.ContentRootPath, "MyDataBaseConnection.json");
            if(File.Exists(path))
            {
                return path;
            }
            path = Path.Combine(env.ContentRootPath, "DataBaseConnection.json");
            if(File.Exists(path))
            {
                return path;
            }
            throw new Exception("Can not locate MyDataBaseConnection.json or DataBaseConnection.json");
        }
    }
}