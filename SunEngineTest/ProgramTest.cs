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
            Program.SetUpConfigurationDirectory(new string[0]);
            Assert.Equal(GetFullPath(DefaultConfigurationFileName), Program.configDir);
        }

        [Fact]
        public void ShouldSetConfigPathToDefaultIfConfigurationPropertyNotPassed()
        {
            Program.SetUpConfigurationDirectory(new[] {InvalidConfigurationProperty});
            Assert.Equal(GetFullPath(DefaultConfigurationFileName), Program.configDir);
        }

        [Fact]
        public void ShouldSetConfigPathToDefaultIfConfigurationPropertyPassedWithEmptyValue()
        {
            Program.SetUpConfigurationDirectory(new[] {ValidConfigurationProperty + ""});
            Assert.Equal(GetFullPath(DefaultConfigurationFileName), Program.configDir);
        }

        [Fact]
        public void ShouldSetConfigPathToDefaultIfConfigurationPropertyPassedWithBlankValue()
        {
            Program.SetUpConfigurationDirectory(new[] {ValidConfigurationProperty + " "});
            Assert.Equal(GetFullPath(DefaultConfigurationFileName), Program.configDir);
        }

        [Fact]
        public void ShouldSetConfigPathToPropertyValueIfItPassed()
        {
            Program.SetUpConfigurationDirectory(new[] {ValidConfigurationProperty + ValidConfigurationFileName});
            Assert.Equal(GetFullPath(ValidConfigurationFileName), Program.configDir);
        }

        private string GetFullPath(string configPath)
        {
            return Path.GetFullPath(configPath);
        }
    }
}