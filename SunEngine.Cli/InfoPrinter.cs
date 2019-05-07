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
        public void PrintStartWithNoArgumentsInfo()
        {
            Console.WriteLine("Startup arguments wasn't provided. To list available commands use 'help' argument.");
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
    test-dbcon                  check is data base connection is working                     
    version                     print SunEngine version
    help                        show this help   
    
Seed test data commands:    
    seed:<CategoryName>:<mCount>:<cCount>      
                                seed category and all subcategories with materials and comments
                                mCount - materials count, default if skipped
                                cCount - comments count, default if skipped
                                example - seed:SomeCategory:20:10
                                
    append-cat-name             add category name to material titles on 'seed'

Examples:
    dotnet SunEngine.dll server
    dotnet SunEngine.dll server config:local.Config.MySite
    dotnet SunEngine.dll migrate init seed
    dotnet SunEngine.dll migrate init seed config:local.Config.MySite
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