using System;
using System.Diagnostics;
using System.Reflection;

namespace SunEngine.Cli
{
    public class InfoPrinter
    {
        /// <summary>
        /// Print this info if "dotnet SunEngine.dll" starts with no commands.
        /// </summary>
        public void PrintVoidStartInfo()
        {
            Console.WriteLine(
                "You start \"SunEngine.dll\" with out arguments.\nRun with \"help\" argument to list commands:\n\tdotnet SunEngine.dll help");
        }

        /// <summary>
        /// Print help on dotnet "dotnet SunEngine.dll help"
        /// </summary>
        public void PrintHelp()
        {
            var helpText = @"
Commands:
    server                      host server api with kestrel
    config:<path>               path to config directory, if none 'Config' is default 
    migrate                     make initial database table structure and migrations in existing database
    init                        initialize users, roles and categories tables from config directory
    version                     print SunEngine version
    help                        show this help   
    
Seed test data commands:    
    seed:<CatName>:<X>:<Y>      seed category and all subcategories with materials and comments
                                CatName - category name, 'Root' if skipped
                                X - materials count, default if skipped
                                Y - comments count, default if skipped
                                
    append-cat-name             add category name to material titles on 'seed'

Examples:
    dotnet SunEngine.dll server
    dotnet SunEngine.dll server config:local.Config.MySite
    dotnet SunEngine.dll migrate init seed
    dotnet SunEngine.dll seed:Forum:10:10
";
            Console.WriteLine(helpText);
        }

        /// <summary>
        /// Print version of SunEngine
        /// </summary>
        public void PrintVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fileVersionInfo.ProductVersion;
            Console.WriteLine("SunEngine Version: " + version);
        }
    }
}