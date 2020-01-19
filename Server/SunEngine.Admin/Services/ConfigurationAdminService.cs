using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Services
{
	public class ConfigurationAdminService
	{
		protected IPathService pathService { get; }
		protected IConfigurationRoot configurationRoot { get; }
		protected IOptionsMonitor<GlobalOptions> globalOptions { get; }

		protected Random ran = new Random();
		
		public ConfigurationAdminService(
			IPathService pathService,
			IConfigurationRoot configurationRoot)
		{
			this.configurationRoot = configurationRoot;
			this.pathService = pathService;
		}

		public void ReloadConfigurationOptions()
		{
			configurationRoot.Reload();
		}

		public void UpdateClientScripts()
		{
			if (!bool.TryParse(configurationRoot.GetSection("Global")["UpdateClientScriptsOnConfigChanges"],
				    out bool update)
			    || update == false)
				return;

			var itemsToSaveDic = new Dictionary<string, Type>()
			{
				["Global:SiteName"] = typeof(string),
				["Global:Locale"] = typeof(string),
				["Global:SiteApi"] = typeof(string),
				["Global:SiteUrl"] = typeof(string),
				["Global:UploadImagesUrl"] = typeof(string),
				["Global:SkinsUrl"] = typeof(string),
				["Global:PartialSkinsUrl"] = typeof(string),
				["Global:CustomCssUrl"] = typeof(string),
				
				["Dev:LogInitExtended"] = typeof(bool),
				["Dev:VueDevTools"] = typeof(bool),
				["Dev:LogRequests"] = typeof(bool),
				["Dev:LogMoveTo"] = typeof(bool),
			};


			var rez = new Dictionary<string, object>();
			foreach (var (key, type) in itemsToSaveDic)
			{
				var value = configurationRoot.GetValue(type, key);

				string[] tokens = key.Split(":");

				Dictionary<string, object> current = rez;
				for (int i = 0; i < tokens.Length - 1; i++)
				{
					if (!current.ContainsKey(tokens[i]))
						current[tokens[i]] = new Dictionary<string, object>();

					current = (Dictionary<string, object>) current[tokens[i]];
				}

				current[tokens[^1]] = value;
			}


			var json = JsonSerializer.Serialize(rez, new JsonSerializerOptions
			{
				WriteIndented = true,
				AllowTrailingCommas = true,
			});

			var configJsPath = pathService.Combine(PathNames.WwwRootDirName, PathNames.StaticsDirName, PathNames.ConfigJsFileName);

			json = json.Substring(1, json.Length - 2) + ",";

			var configJs = File.ReadAllText(configJsPath);
			configJs = Regex.Replace(configJs, "//( *?)auto-start(.*?)//( *?)auto-end",
				$"// auto-start\n{json}\n // auto-end", RegexOptions.Singleline);
			File.WriteAllText(configJsPath, configJs);

			UpdateConfigVersion();
		}

		public void UpdateConfigVersion()
		{
			var indexHtmlPath = pathService.Combine(PathNames.WwwRootDirName, "index.html");
			string text = File.ReadAllText(indexHtmlPath);
			Regex reg = new Regex("configver=\\d+\"");
			text = reg.Replace(text, $"configver={ran.Next()}\"");
			File.WriteAllText(indexHtmlPath, text);
		}
		
		public void UpdateCustomCssVersion()
		{
			UpdateConfigVersion();
			
			var configJsPath =  pathService.Combine(PathNames.WwwRootDirName,PathNames.StaticsDirName ,PathNames.ConfigJsFileName);
			string text = File.ReadAllText(configJsPath);
			Regex reg = new Regex("customcssver=\\d+\"");
			text = reg.Replace(text, $"customcssver={ran.Next()}\"");
			File.WriteAllText(configJsPath, text);
		}
	}
}