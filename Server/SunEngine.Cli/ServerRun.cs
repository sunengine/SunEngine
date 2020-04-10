using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Namotion.Reflection;
using SunEngine.Core.Configuration;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Services;

namespace SunEngine.Cli
{
	public class ServerRun
	{
		public void RunServer(StartupConfiguration startupConfiguration)
		{
			var webHost = CreateHostBuilder(startupConfiguration).Build();

			IWebHostEnvironment env = (IWebHostEnvironment) webHost.Services.GetService(typeof(IWebHostEnvironment));
			IConfiguration conf = (IConfiguration) webHost.Services.GetService(typeof(IConfiguration));

			Startup.SetExceptionsMode(env, conf);

			new InfrastructurePreparer((IConfigurationRoot) conf).DoAll();

			string endPoint = conf["Kestrel:EndPoints:Http:Url"];
			if(endPoint != null)
				Console.WriteLine("Server API run at: " + endPoint);
			
			Console.WriteLine();

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
							PathNames.DataBaseConnectionJsonFileName));
						string mainSettingsFile =
							Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir,
								PathNames.SunEngineJsonFileName));
						string logSettingsFile =
							Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir,
								PathNames.LogConfigJsonFileName));
            string customConfigFile = Path.GetFullPath(Path.Combine(startupConfiguration.ConfigRootDir,
              PathNames.CustomConfigJsonFileName));

						config.AddJsonFile(logSettingsFile, false, false);
						config.AddJsonFile(dbSettingFile, false, false);
						config.AddJsonFile(mainSettingsFile, false, false);
            config.AddJsonFile(customConfigFile, false, false);
            
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
            
						config.AddInMemoryCollection(new[]
						{
							new KeyValuePair<string, string>("Dirs:Config",
								Path.GetFullPath(startupConfiguration.ConfigRootDir))
						});

						config.AddCommandLine(startupConfiguration.Arguments);
					});


					void UseStaticFiles()
					{
						var configurationRoot = PathService.MakeConfiguration(startupConfiguration.ConfigRootDir);
						var fileServer = configurationRoot["Server:FileServer"];
						if (fileServer != null && bool.Parse(fileServer))
							builder.UseWebRoot(new PathService(startupConfiguration.ConfigRootDir).WwwRootDir);
					}
				});
		}
	}
}
