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
                        Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir, "SanitizerOptions.json"));
                    
                    config.AddJsonFile(logSettingsFile, false, false);
                    config.AddJsonFile(dbSettingFile, false, false);
                    config.AddJsonFile(mainSettingsFile, false, false);
                    config.AddJsonFile(sanitizerOptionsFile, false, false);
                    config.AddCommandLine(startupConfiguration.Arguments);
                });
    }
 
}
