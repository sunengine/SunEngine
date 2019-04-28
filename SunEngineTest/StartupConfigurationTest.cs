using System.IO;
using SunEngine;
using Xunit;

namespace SunEngineTest
{
    public class UnitFact
    {
        private const string DefaultConfigurationFileName = "Config";
        private const string InvalidConfigurationProperty = "invalidProperty:";
        private const string ValidConfigurationProperty = "config:";
        private const string ValidConfigurationFileName = "ConfigurationFileName";

        [Fact]
        public void ShouldSetConfigPathToDefaultIfPropertiesNotPassed()
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(new string[0]);
            Assert.Equal(GetFullPath(DefaultConfigurationFileName), startupConfiguration.ConfigurationDirectoryRoute);
        }

        [Fact]
        public void ShouldSetConfigPathToDefaultIfConfigurationPropertyNotPassed()
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(new[] {InvalidConfigurationProperty});
            Assert.Equal(GetFullPath(DefaultConfigurationFileName), startupConfiguration.ConfigurationDirectoryRoute);
        }

        [Fact]
        public void ShouldSetConfigPathToDefaultIfConfigurationPropertyPassedWithEmptyValue()
        {
            StartupConfiguration startupConfiguration =
                new StartupConfiguration(new[] {ValidConfigurationProperty + ""});
            Assert.Equal(GetFullPath(DefaultConfigurationFileName), startupConfiguration.ConfigurationDirectoryRoute);
        }

        [Fact]
        public void ShouldSetConfigPathToDefaultIfConfigurationPropertyPassedWithBlankValue()
        {
            StartupConfiguration startupConfiguration =
                new StartupConfiguration(new[] {ValidConfigurationProperty + " "});
            Assert.Equal(GetFullPath(DefaultConfigurationFileName), startupConfiguration.ConfigurationDirectoryRoute);
        }

        [Fact]
        public void ShouldSetConfigPathToPropertyValueIfItPassed()
        {
            StartupConfiguration startupConfiguration =
                new StartupConfiguration(new[] {ValidConfigurationProperty + ValidConfigurationFileName});
            Assert.Equal(GetFullPath(ValidConfigurationFileName), startupConfiguration.ConfigurationDirectoryRoute);
        }

        private string GetFullPath(string configPath)
        {
            return Path.GetFullPath(configPath);
        }
    }
}
