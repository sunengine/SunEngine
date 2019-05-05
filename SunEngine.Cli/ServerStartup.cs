using System;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SunEngine.Cli
{
    public class ServerStartup
    {
        public void RunServer(StartupConfiguration startupConfiguration)
        {
            var webHost = CreateWebHostBuilder(startupConfiguration).Build();
                
            IHostingEnvironment env = (IHostingEnvironment) webHost.Services.GetService(typeof(IHostingEnvironment));
            IConfiguration conf = (IConfiguration) webHost.Services.GetService(typeof(IConfiguration));
            
            Startup.SetExceptionsMode(env, conf);
            webHost.Run();
        }

        private IWebHostBuilder CreateWebHostBuilder(StartupConfiguration startupConfiguration) =>
            WebHost.CreateDefaultBuilder(startupConfiguration.Arguments)
                .UseKestrel()
                .UseStartup<Startup>()
                .ConfigureAppConfiguration((builderContext, config) =>
                {
                    string dbSettingFile = Path.GetFullPath(Path.Combine(startupConfiguration.ConfigurationDirectoryRoute, "DataBaseConnection.json"));
                    string mainSettingsFile = Path.GetFullPath(Path.Combine(startupConfiguration.ConfigurationDirectoryRoute, "SunEngine.json"));
                    string logSettingsFile = Path.GetFullPath(Path.Combine(startupConfiguration.ConfigurationDirectoryRoute, "LogConfig.json"));

                    config.AddJsonFile(logSettingsFile, false, false);
                    config.AddJsonFile(dbSettingFile, false, false);
                    config.AddJsonFile(mainSettingsFile, false, false);
                    config.AddCommandLine(startupConfiguration.Arguments);
                });
    }
 
}
