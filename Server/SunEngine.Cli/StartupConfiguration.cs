using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SunEngine.Core.Errors.Exceptions;
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
                ConfigRootDir = FindDefaultConfigPath();
            }
            else
            {
                var configurationDirectory = configurationArg.Substring(configArgPrefix.Length).Trim();

                var dirName = Path.GetFileName(configurationDirectory);
                if (!dirName.Equals(DefaultConfigFileName) &&
                    !dirName.EndsWith(ConfigDirectoryNameEnd))
                    configurationDirectory += ConfigDirectoryNameEnd;

                TestIfConfigurationDirectoryExists(configurationDirectory);
                ConfigRootDir = Path.GetFullPath(configurationDirectory);
            }

            Console.WriteLine($@"Using configuration directory ""{ConfigRootDir}"".");


            void TestIfConfigurationDirectoryExists(string dirPath)
            {
                if (Directory.Exists(dirPath))
                    return;

                Console.WriteLine($@"Configuration directory ""{dirPath}"" does not exists.");
                Environment.Exit(1);
            }

            string FindDefaultConfigPath()
            {
                List<string> dirTokens = Directory.GetCurrentDirectory().Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).ToList();
                for (int i = 0; i < 3; i++)
                {
                    var currentPath = string.Join(Path.DirectorySeparatorChar, dirTokens);
                    if (CheckDir(currentPath))
                        return Path.Combine(currentPath, DefaultConfigFileName);
                    if (dirTokens.Count >= 2)
                        dirTokens.RemoveAt(dirTokens.Count - 1);
                    else
                        break;
                }

                throw new SunException("Can not find Config directory");

                bool CheckDir(string path) => File.Exists(Path.Combine(path, DefaultConfigFileName,
                    "SunEngine.json"));
            }
        }
    }
}