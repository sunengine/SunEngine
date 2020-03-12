using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SunEngine.Core.Errors.Exceptions;
using Path = System.IO.Path;

namespace SunEngine.Core.Services
{
	public interface IPathService
	{
		string MakePath(string token);
		string GetPath(string dirName);
		string Combine(string dirName, params string[] args);

		string WwwRootDir { get; }
		string ApplicationDir { get; }
		string ConfigDir { get; }
	}

	public static class PathNames
	{
		public const string ApplicationRootName = "ApplicationRoot";
		public const string WwwRootDirName = "WwwRoot";
		public const string ConfigDirName = "Config";
		public const string ResourcesDirName = "Resources";
		public const string SkinsDirName = "Skins";
		public const string PartialSkinsDirName = "PartialSkins";
		public const string UploadImagesDirName = "UploadImages";
		public const string MailTemplatesDirName = "MailTemplates";
		public const string CustomCssFileName = "custom.css";
		public const string CustomJavaScriptFileName = "custom.js";
		public const string ConfigJsFileName = "config.js";
		public const string ConfigTemplateJsFileName = "config.template.js";
		public const string ServerInfoJsonFileName = "ServerInfo.json";
		public const string SunEngineJsonFileName = "SunEngine.json";
		public const string LogConfigJsonFileName = "LogConfig.json";
		public const string RootDetectFileName = "SunEngine.md";
		public const string IndexHtmlFileName = "index.html";
		public const string DataBaseConnectionJsonFileName = "DataBaseConnection.json";
	}

	public class PathService : IPathService
	{
		public string WwwRootDir { get; private set; }
		public string ApplicationDir { get; private set; }
		public string ConfigDir { get; private set; }

		protected Dictionary<string, string> Pathes;

		public PathService(IConfiguration configuration)
		{
			Init(configuration);
		}

		public PathService(string configDir)
		{
			Init(MakeConfiguration(configDir));
		}

		public static IConfigurationRoot MakeConfiguration(string configDir)
		{
			ConfigurationBuilder cb = new ConfigurationBuilder();
			cb.AddJsonFile(Path.Combine(configDir, PathNames.SunEngineJsonFileName), false);
			cb.AddInMemoryCollection(new[]
			{
				new KeyValuePair<string, string>("Dirs:Config", configDir)
			});
			return cb.Build();
		}

		private void Init(IConfiguration configuration)
		{
			Pathes = new Dictionary<string, string>();

			var dirsSection = configuration.GetSection("Dirs");
			var dic = dirsSection.GetChildren().ToDictionary(x => x.Key, x => x.Value);

			if (!dic.TryGetValue(PathNames.ConfigDirName, out string configToken))
				throw new SunException($"No {PathNames.ConfigDirName} in config Global section");

			if (!dic.TryGetValue(PathNames.ApplicationRootName, out string applicationRootToken))
				throw new SunException($"No {PathNames.ApplicationRootName} in config Global section");
			if (applicationRootToken == "auto")
				ApplicationDir = SearchApplicationRootDir(configToken);
			else
				ApplicationDir = applicationRootToken;

			if (!dic.TryGetValue(PathNames.WwwRootDirName, out string wwwRootDirToken))
				throw new SunException($"No {PathNames.WwwRootDirName} in config Global section");
			WwwRootDir = MakePath(wwwRootDirToken);

			ConfigDir = MakePath(configToken);

			foreach (var (key, value) in dic)
				Pathes.Add(key, MakePath(value));
		}

		public static string SearchApplicationRootDir(string configPath)
		{
			string[] dirsToSearch = {Directory.GetCurrentDirectory(), configPath};

			foreach (string dir in dirsToSearch)
			{
				var dirTokens = dir.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToList();
				for (int i = 0; i < 5; i++)
				{
					var currentPath = string.Join(Path.DirectorySeparatorChar, dirTokens);
					if (CheckDir(currentPath))
						return currentPath;

					if (dirTokens.Count >= 2)
						dirTokens.RemoveAt(dirTokens.Count - 1);
					else
						break;
				}
			}

			throw new SunException("Can not find Application root directory");

			bool CheckDir(string path) => File.Exists(Path.Combine(path, PathNames.RootDetectFileName));
		}

		private const string WwwRootDirPrefix = "%wwwroot%";
		private const string AppDirPrefix = "%app%";
		private const string ConfigDirPrefix = "%config%";

		public string GetPath(string dirName)
		{
			if (Pathes.TryGetValue(dirName, out var value))
				return value;

			throw new SunException($"No directory {dirName} in " + PathNames.SunEngineJsonFileName + " Dirs");
		}

		public string Combine(string dirName, params string[] args)
		{
			var dirPath = GetPath(dirName);
			return Path.Combine(dirPath, System.IO.Path.Combine(args));
		}

		public string MakePath(string token)
		{
			if (token.StartsWith(AppDirPrefix))
				return Combine(ApplicationDir, Split(token.Substring(AppDirPrefix.Length + 1)));
			if (token.StartsWith(WwwRootDirPrefix))
				return Combine(WwwRootDir, Split(token.Substring(WwwRootDirPrefix.Length + 1)));
			if (token.StartsWith(ConfigDirPrefix))
				return Combine(ConfigDir, Split(token.Substring(ConfigDirPrefix.Length + 1)));

			return token;

			string[] Split(string tkn) => tkn.Split('\\', '/');
			string Combine(string start, string[] next) => System.IO.Path.Combine(start, System.IO.Path.Combine(next));
		}
	}
}