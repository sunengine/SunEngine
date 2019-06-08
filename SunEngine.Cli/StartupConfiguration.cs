using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SunEngine.Cli
{
    /// <summary>
    /// Translates arguments into configuration properties.
    /// </summary>
    public class StartupConfiguration
    {
        public const string ConfigArgumentName = "config:";
        public const string DefaultConfigFileName = "Config";
        public const string ConfigDirectoryNameEnd = ".Config";

        public const string HelpCommand = "help";
        public const string VersionCommand = "version";
        public const string ServerCommand = "server";
        public const string MigrateCommand = "migrate";
        public const string InitCommand = "init";
        public const string SeedCommand = "seed";
        public const string AppendCategoriesNamesCommand = "append-cat-name";
        public const string TestDatabaseConnection = "check-db-con";


        public string[] Arguments { get; }
        public string ConfigRootDir { get; }
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
            
            SeedWithTestData = startupArguments.Contains(SeedCommand);
            SeedWithCategoryNames = startupArguments.Contains(AppendCategoriesNamesCommand);
            CheckDatabaseAvailability = startupArguments.Contains(TestDatabaseConnection);
            
            // TODO Revisit logic of category tokens. It could be seed:catToken, seed-catToken etc. Write logic to contain only category names, without "seed"
            CategoryTokensToSeed = startupArguments.Where(x => x.StartsWith(SeedCommand)).ToList();
            
            var configurationDirectory = GetConfigurationDirectory(arguments);
            ConfigRootDir = Path.GetFullPath(configurationDirectory);
        }

        private string GetConfigurationDirectory(IEnumerable<string> arguments)
        {
            var configurationProperty = arguments.FirstOrDefault(x => x.StartsWith(ConfigArgumentName));
            if (string.IsNullOrEmpty(configurationProperty))
            {
                Console.WriteLine("Property for configuration wasn't set. Default configuration will be used.");
                return DefaultConfigFileName;
            }

            var configurationFileName = configurationProperty.Substring(ConfigArgumentName.Length).Trim();
            if (string.IsNullOrEmpty(configurationFileName))
            {
                Console.WriteLine("Property for configuration was empty or blank. Default configuration will be used.");
                return DefaultConfigFileName;
            }

            if (!configurationFileName.Equals(DefaultConfigFileName) && !configurationFileName.EndsWith(ConfigDirectoryNameEnd))
            {
                configurationFileName += ConfigDirectoryNameEnd;
            }

            Console.WriteLine($"Configuration directory \"{configurationFileName}\" will be used.");
            return configurationFileName;
        }
    }
}
