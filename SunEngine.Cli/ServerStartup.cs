using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SunEngine.Core.Configuration;
using SunEngine.Core.DataBase;

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


        private IHostBuilder CreateHostBuilder(StartupConfiguration startupConfiguration)
        {
            return Host.CreateDefaultBuilder(startupConfiguration.Arguments)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseKestrel();
                    builder.UseStartup<Startup>();
                    builder.ConfigureAppConfiguration((builderContext, config) =>
                    {
                        string dbSettingFile = Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir,
                            "DataBaseConnection.json"));
                        string mainSettingsFile =
                            Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir, "SunEngine.json"));
                        string logSettingsFile =
                            Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir, "LogConfig.json"));
                        //string sanitizerOptionsFile = Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir, "Sanitizer.json"));

                        config.AddJsonFile(logSettingsFile, false, false);
                        config.AddJsonFile(dbSettingFile, false, false);
                        config.AddJsonFile(mainSettingsFile, false, false);
                        //config.AddJsonFile(sanitizerOptionsFile, false, false);

                        var dataBaseConnectionObject = JObject.Parse(File.ReadAllText(dbSettingFile));
                        var dataBaseConnectionVars = dataBaseConnectionObject["DataBaseConnection"];
                        var linq2dbProvider = dataBaseConnectionVars["Linq2dbProvider"].ToString();
                        var connectionString = dataBaseConnectionVars["ConnectionString"].ToString();

                        DataBaseFactory.DefaultDataBaseFactory = new DataBaseFactory(linq2dbProvider, connectionString,
                            new DbMappingSchema());

                        ConfigDbProvider.DefaultConfigDbProvider =
                            new ConfigDbProvider(DataBaseFactory.DefaultDataBaseFactory);

                        config.Add(new ConfigDbSource(ConfigDbProvider.DefaultConfigDbProvider,
                            DataBaseFactory.DefaultDataBaseFactory));

                        config.AddInMemoryCollection(new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("Global:ConfigRootDir",  Path.GetFullPath(startupConfiguration.ConfigRootDir))
                        });

                        config.AddCommandLine(startupConfiguration.Arguments);
                    });
                });
        }
    }
}