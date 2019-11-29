using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SunEngine.Core.Configuration;
using SunEngine.Core.DataBase;
using SunEngine.Core.Services;

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

            new InfrastructurePreparer((IConfigurationRoot) conf).DoAll();

            webHost.Run();
        }


        private IHostBuilder CreateHostBuilder(StartupConfiguration startupConfiguration)
        {
            JsonDocumentOptions jOptions = new JsonDocumentOptions
            {
                CommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            return Host.CreateDefaultBuilder(startupConfiguration.Arguments)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.UseKestrel();
                    builder.UseStartup<Startup>();

                    UseStaticFiles();

                    builder.ConfigureAppConfiguration((builderContext, config) =>
                    {
                        string dbSettingFile = Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir,
                            "DataBaseConnection.json"));
                        string mainSettingsFile =
                            Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir, "SunEngine.json"));
                        string logSettingsFile =
                            Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir, "LogConfig.json"));

                        config.AddJsonFile(logSettingsFile, false, false);
                        config.AddJsonFile(dbSettingFile, false, false);
                        config.AddJsonFile(mainSettingsFile, false, false);


                        var dataBaseConnectionObject = JsonDocument.Parse(File.ReadAllText(dbSettingFile), jOptions);
                        var dataBaseConnectionVars =
                            dataBaseConnectionObject.RootElement.GetProperty("DataBaseConnection");
                        var linq2dbProvider = dataBaseConnectionVars.GetProperty("Linq2dbProvider").GetString();
                        var connectionString = dataBaseConnectionVars.GetProperty("ConnectionString").GetString();

                        DataBaseFactory.DefaultDataBaseFactory = new DataBaseFactory(linq2dbProvider, connectionString,
                            new DbMappingSchema());

                        ConfigDbProvider.DefaultConfigDbProvider =
                            new ConfigDbProvider(DataBaseFactory.DefaultDataBaseFactory);

                        config.Add(new ConfigDbSource(ConfigDbProvider.DefaultConfigDbProvider,
                            DataBaseFactory.DefaultDataBaseFactory));

                        config.AddInMemoryCollection(new List<KeyValuePair<string, string>>()
                        {
                            new KeyValuePair<string, string>("Dirs:Config",
                                Path.GetFullPath(startupConfiguration.ConfigRootDir))
                        });

                        config.AddCommandLine(startupConfiguration.Arguments);
                    });


                    void UseStaticFiles()
                    {
                        var sunEngineJsonDocument =
                            JsonDocument.Parse(
                                File.ReadAllText(Path.Combine(startupConfiguration.ConfigRootDir, "SunEngine.json")),
                                jOptions);

                        var wwwRootDir = sunEngineJsonDocument.RootElement.GetProperty("Dirs").GetProperty("WwwRoot")
                            .GetString();

                        if (sunEngineJsonDocument.RootElement.GetProperty("Global").GetProperty("FileServer")
                            .GetBoolean())
                        {
                            if (wwwRootDir.StartsWith("%app%"))
                            {
                                var wwwRootDirTokens = wwwRootDir.Substring("%app%".Length + 1).Split('\\', '/');
                                var tokens = new List<string>(wwwRootDirTokens.Length + 1)
                                    {PathService.SearchApplicationRootDir()};
                                tokens.AddRange(wwwRootDirTokens);
                                wwwRootDir = Path.Combine(tokens.ToArray());
                            }

                            builder.UseWebRoot(wwwRootDir);
                        }
                    }
                });
        }
    }
}