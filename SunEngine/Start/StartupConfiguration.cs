using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SunEngine.Start
{
    public class StartupConfiguration
    {
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
                Console.Write("Property for configuration wasn't set. Default configuration will be used.");
                return DefaultConfigurationFileName;
            }

            var configurationFileName = configurationProperty.Substring(ConfigurationArgumentName.Length).Trim();
            if (string.IsNullOrEmpty(configurationFileName))
            {
                Console.Write("Property for configuration was empty or blank. Default configuration will be used.");
                return DefaultConfigurationFileName;
            }

            Console.Write($"Configuration file {configurationFileName} will be used.");
            return configurationFileName;
        }
    }
    
}
