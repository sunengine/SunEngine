using System.Collections.Generic;
using System.IO;
using System.Linq;
using NLog;

namespace SunEngine
{
    /// <summary>
    /// Translates arguments into configuration properties.
    /// </summary>
    public class StartupConfiguration
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        
        private const string ConfigurationArgumentName = "config:";
        private const string DefaultConfigurationFileName = "Config";

        private const string HelpCommand = "help";
        private const string VersionCommand = "version";
        private const string ServerCommand = "server";
        private const string MigrateCommand = "migrate";
        private const string InitCommand = "init";
        private const string SeedCommand = "seed";
        private const string AppendCategoriesNamesCommand = "append-cat-name";

        public string[] Arguments { get; }
        public string ConfigurationDirectoryRoute { get; }
        public bool PrintHelp { get; }
        public bool PrintVersion { get; }
        public bool StartServer { get; }

        public bool Migrate { get; }
        public bool InitializeCoreData { get; }
        
        public bool SeedWithTestData { get; }
        public bool SeedWithCategoryNames { get; }
        public List<string> CategoryTokensToSeed { get; }

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
            
            // TODO Revisit logic of category tokens. It could be seed:catToken, seed-catToken etc. Write logic to contain only category names, without "seed"
            CategoryTokensToSeed = startupArguments.Where(x => x.StartsWith(SeedCommand)).ToList();
            
            var configurationDirectory = GetConfigurationDirectory(arguments);
            ConfigurationDirectoryRoute = Path.GetFullPath(configurationDirectory);
        }

        private string GetConfigurationDirectory(IEnumerable<string> arguments)
        {
            var configurationProperty = arguments.FirstOrDefault(x => x.StartsWith(ConfigurationArgumentName));
            if (string.IsNullOrEmpty(configurationProperty))
            {
                Logger.Warn("Property for configuration wasn't set. Default configuration will be used.");
                return DefaultConfigurationFileName;
            }

            var configurationFileName = configurationProperty.Substring(ConfigurationArgumentName.Length).Trim();
            if (string.IsNullOrEmpty(configurationFileName))
            {
                Logger.Warn("Property for configuration was empty or blank. Default configuration will be used.");
                return DefaultConfigurationFileName;
            }

            Logger.Info($"Configuration file {configurationFileName} will be used.");
            return configurationFileName;
        }
    }
    
}
