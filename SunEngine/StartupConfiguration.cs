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

        public string ConfigurationDirectoryRoute { get; }

        public StartupConfiguration(IEnumerable<string> arguments)
        {
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