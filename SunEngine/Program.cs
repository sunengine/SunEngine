using System;
using System.IO;
using System.Net;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

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
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Loopback, 5000);
                })
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    IHostingEnvironment env = builderContext.HostingEnvironment;
                    
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddJsonFile("SunEngine.json", optional: false, reloadOnChange: true);
                    
                    if (File.Exists(Path.Combine(env.ContentRootPath,"MyDataBaseConnection.json")))
                    {
                        config.AddJsonFile($"MyDataBaseConnection.json", optional: false, reloadOnChange: true);
                    }
                    else
                    {
                        config.AddJsonFile($"DataBaseConnection.json", optional: false, reloadOnChange: true);
                    }
                });
    }
}