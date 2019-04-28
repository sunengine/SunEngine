using System.IO;
using SunEngine;
using Xunit;

namespace SunEngineTest
{
    public class StartupConfigurationTest
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

        [Fact]
        public void ShouldSetPrintHelpToTrueIfArgumentPassed()
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(new[] {"help"});
            Assert.True(startupConfiguration.PrintHelp);
        }

        [Fact]
        public void ShouldSetPrintHelpToFalseIfInvalidArgumentPassed()
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(new[] {"help123"});
            Assert.False(startupConfiguration.PrintHelp);
        }
        
        [Fact]
        public void ShouldSetPrintVersionToTrueIfArgumentPassed()
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(new[] {"version"});
            Assert.True(startupConfiguration.PrintVersion);
        }

        [Fact]
        public void ShouldSetPrintVersionToFalseIfInvalidArgumentPassed()
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(new[] {"version123"});
            Assert.False(startupConfiguration.PrintHelp);
        }
        
        [Fact]
        public void ShouldSetStartServerToTrueIfArgumentPassed()
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(new[] {"server"});
            Assert.True(startupConfiguration.StartServer);
        }
        
        [Fact]
        public void ShouldSetStartServerToFalseIfInvalidArgumentPassed()
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(new[] {"server123"});
            Assert.False(startupConfiguration.StartServer);
        }

        [Fact]
        public void ShouldSetMigrateToTrueIfArgumentPassed()
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(new[] {"migrate"});
            Assert.True(startupConfiguration.Migrate);
        }
        
        [Fact]
        public void ShouldSetMigrateToFalseIfInvalidArgumentPassed()
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(new[] {"migrate123"});
            Assert.False(startupConfiguration.StartServer);
        }

        [Fact]
        public void ShouldSetInitializeCoreDataToTrueIfArgumentPassed()
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(new[] {"init"});
            Assert.True(startupConfiguration.InitializeCoreData);
        }
        
        [Fact]
        public void ShouldSetInitializeCoreDataToFalseIfInvalidArgumentPassed()
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(new[] {"init123"});
            Assert.False(startupConfiguration.InitializeCoreData);
        }

        [Fact]
        public void ShouldSetSeedToTrueIfArgumentPassed()
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(new[] {"seed"});
            Assert.True(startupConfiguration.SeedWithTestData);
        }
        
        [Fact]
        public void ShouldSetSeedWithTestDataToFalseIfInvalidArgumentPassed()
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(new[] {"seed123"});
            Assert.False(startupConfiguration.SeedWithTestData);
        }

        [Fact]
        public void ShouldSetCategoryNamesToSeedIfArgumentPassed()
        {
            // TODO Add test for cases when name provided without ':', when no names provided or several names provided. 
            StartupConfiguration startupConfiguration = new StartupConfiguration(new[] {"seed:category-name"});
            Assert.Contains("seed:category-name", startupConfiguration.CategoryTokensToSeed);
        }

        [Fact]
        public void ShouldSetSeedWithCategoryNamesToTrueIfArgumentPassed()
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(new[] {"append-cat-name"});
            Assert.True(startupConfiguration.SeedWithCategoryNames);
        }
        
        [Fact]
        public void ShouldSetSeedWithCategoryNamesToFalseIfInvalidArgumentPassed()
        {
            StartupConfiguration startupConfiguration = new StartupConfiguration(new[] {"append-cat-name123"});
            Assert.False(startupConfiguration.SeedWithCategoryNames);
        }

        private string GetFullPath(string configPath)
        {
            return Path.GetFullPath(configPath);
        }
    }
}