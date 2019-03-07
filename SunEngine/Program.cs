using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SunEngine.Configuration;
using SunEngine.Utils;

//[assembly:ApiController]

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