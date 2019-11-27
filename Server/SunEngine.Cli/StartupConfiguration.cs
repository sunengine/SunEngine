using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SixLabors.Shapes;
using Path = System.IO.Path;

namespace SunEngine.Cli
{
    /// <summary>
    /// Translates arguments into configuration properties.
    /// </summary>
    public class StartupConfiguration
    {
        public const string ConfigArgumentName = "config";
        public const string DefaultConfigFileName = "Config";
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

        public void InitConfigurationDirectory()
        {
            const string configArgPrefix = ConfigArgumentName + ":";
            var configurationArg = Arguments.FirstOrDefault(x => x.StartsWith(configArgPrefix));
            if (string.IsNullOrEmpty(configurationArg))
            {
                var configDir = Path.GetFullPath(DefaultConfigFileName);
                Console.WriteLine($@"Using configuration directory ""{configDir}"".");
                TestIfConfigurationDirectoryExists(configDir);
                ConfigRootDir = configDir;
                return;
            }

            var configurationDirectory = configurationArg.Substring(configArgPrefix.Length).Trim();

            if (!configurationDirectory.Equals(DefaultConfigFileName) &&
                !configurationDirectory.EndsWith(ConfigDirectoryNameEnd))
                configurationDirectory += ConfigDirectoryNameEnd;
            
            TestIfConfigurationDirectoryExists(configurationDirectory);
            Console.WriteLine($@"Using configuration directory ""{configurationDirectory}"".");
            ConfigRootDir = Path.GetFullPath(configurationDirectory);

            void TestIfConfigurationDirectoryExists(string dirPath)
            {
                if (Directory.Exists(dirPath))
                    return;

                Console.WriteLine($@"Configuration directory ""{dirPath}"" does not exists.");
                Environment.Exit(1);
            }
        }
    }
}