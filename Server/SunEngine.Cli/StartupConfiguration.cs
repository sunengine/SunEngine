using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Path = System.IO.Path;

namespace SunEngine.Cli
{
	/// <summary>
	/// Translates arguments into configuration properties.
	/// </summary>
	public class StartupConfiguration
	{
		public const string ConfigArgumentName = "config";
		public const string DefaultConfigDirName = "Config";
		public const string ConfigDirectoryNameEnd = ".Config";

		public const string HelpCommand = "help";
		public const string VersionCommand = "version";
		public const string ServerCommand = "server";
		public const string MigrateCommand = "migrate";
		public const string InitCommand = "init";
		public const string SeedCommand = "seed";
		public const string AppendCategoriesNamesCommand = "append-cat-name";
		public const string TestDatabaseConnection = "test-db-con";


		public string[] Arguments { get; }
		public string ConfigRootDir { get; private set; }
		public bool PrintHelp { get; }
		public bool PrintVersion { get; }
		public bool StartServer { get; }

		public bool Migrate { get; }
		public bool InitializeCoreData { get; }

		public bool SeedWithTestData { get; }
		public bool SeedWithCategoryNames { get; }
		public List<string> CategoryTokensToSeed { get; }

		public bool CheckDatabaseAvailability { get; }


		public StartupConfiguration(string[] arguments)
		{
			Arguments = arguments;

			var startupArguments = arguments.ToHashSet();

			PrintHelp = startupArguments.Contains(HelpCommand);
			PrintVersion = startupArguments.Contains(VersionCommand);

			StartServer = startupArguments.Contains(ServerCommand);
			InitializeCoreData = startupArguments.Contains(InitCommand);
			Migrate = startupArguments.Contains(MigrateCommand);

			SeedWithTestData = startupArguments.Any(x => x.StartsWith(SeedCommand));
			SeedWithCategoryNames = startupArguments.Contains(AppendCategoriesNamesCommand);
			CheckDatabaseAvailability = startupArguments.Contains(TestDatabaseConnection);

			CategoryTokensToSeed = startupArguments.Where(x => x.StartsWith(SeedCommand)).ToList();
		}

		public bool CheckNoArguments() => Arguments == null || Arguments.Length == 0;

		// Detect configuration directory and exit if it is not found
		public void InitConfigurationDirectory()
		{
			const string configArgPrefix = ConfigArgumentName + ":";
			var configurationArg = Arguments.FirstOrDefault(x => x.StartsWith(configArgPrefix));
			if (string.IsNullOrEmpty(configurationArg))
			{
				ConfigRootDir = FindConfigPath(DefaultConfigDirName);
			}
			else
			{
				var configurationDirectory = configurationArg.Substring(configArgPrefix.Length).Trim();

				if (!configurationArg.EndsWith(ConfigDirectoryNameEnd)
				    && !configurationArg.EndsWith(Path.DirectorySeparatorChar + DefaultConfigDirName)
				    && !configurationArg.EndsWith(Path.AltDirectorySeparatorChar + DefaultConfigDirName))
					configurationDirectory += ConfigDirectoryNameEnd;

				if (!configurationArg.Contains("..") && !configurationArg.Contains(Path.DirectorySeparatorChar) &&
				    !configurationArg.Contains(Path.AltDirectorySeparatorChar))
				{
					ConfigRootDir = FindConfigPath(configurationDirectory);
				}
				else
				{
					if (!CheckDir(configurationDirectory))
						ConfigNotFound(configurationDirectory);
					ConfigRootDir = configurationDirectory;
				}
			}

			Console.WriteLine("Config folder: " + ConfigRootDir);

			string FindConfigPath(string name)
			{
				List<string> dirTokens = Directory.GetCurrentDirectory()
					.Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToList();
				for (int i = 0; i < 5; i++)
				{
					var currentPath = string.Join(Path.DirectorySeparatorChar, dirTokens);
					currentPath = Path.Combine(currentPath, name);
					if (CheckDir(currentPath))
						return currentPath;
					if (dirTokens.Count >= 2)
						dirTokens.RemoveAt(dirTokens.Count - 1);
					else
						break;
				}

				ConfigNotFound(name);
				return null;
			}

			bool CheckDir(string dirPath) => File.Exists(Path.Combine(dirPath, "SunEngine.json"));

			void ConfigNotFound(string dir)
			{
				Console.WriteLine($@"Can not find configuration directory ""{dir}"".");
				Environment.Exit(1);
			}
		}
	}
}