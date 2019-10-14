using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace SunEngine.Cli
{
    public class ServerStartup
    {
        public void RunServer(StartupConfiguration startupConfiguration)
        {
            var webHost = CreateHostBuilder(startupConfiguration).Build();
                
            IWebHostEnvironment env = (IWebHostEnvironment) webHost.Services.GetService(typeof(IWebHostEnvironment));
            IConfiguration conf = (IConfiguration) webHost.Services.GetService(typeof(IConfiguration));
         
            InfoPrinter.PrintVersion();
            Startup.SetExceptionsMode(env, conf);
            webHost.Run();
        }

        private IWebHostBuilder CreateWebHostBuilder(StartupConfiguration startupConfiguration) =>
            WebHost.CreateDefaultBuilder(startupConfiguration.Arguments)
                .UseKestrel()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    string dbSettingFile = Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir, "DataBaseConnection.json"));
                    string mainSettingsFile = Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir, "SunEngine.json"));
                    string logSettingsFile = Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir, "LogConfig.json"));
                    string sanitizerOptionsFile =
                        Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir, "Sanitizer.json"));
                    
                    config.AddJsonFile(logSettingsFile, false, false);
                    config.AddJsonFile(dbSettingFile, false, false);
                    config.AddJsonFile(mainSettingsFile, false, false);
                    config.AddJsonFile(sanitizerOptionsFile, false, false);
                    config.AddCommandLine(startupConfiguration.Arguments);
                });

        private IHostBuilder CreateHostBuilder(StartupConfiguration startupConfiguration) =>
            Host.CreateDefaultBuilder(startupConfiguration.Arguments)
            .ConfigureWebHostDefaults(builder =>
            {
                builder.UseKestrel();
                builder.UseStartup<Startup>();
                builder.ConfigureAppConfiguration((builder, config) => 
                {
                    string dbSettingFile = Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir, "DataBaseConnection.json"));
                    string mainSettingsFile = Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir, "SunEngine.json"));
                    string logSettingsFile = Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir, "LogConfig.json"));
                    string sanitizerOptionsFile =
                        Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir, "Sanitizer.json"));
                    
                    config.AddJsonFile(logSettingsFile, false, false);
                    config.AddJsonFile(dbSettingFile, false, false);
                    config.AddJsonFile(mainSettingsFile, false, false);
                    config.AddJsonFile(sanitizerOptionsFile, false, false);
                    config.AddCommandLine(startupConfiguration.Arguments);
                });
            });
    }
 
}
