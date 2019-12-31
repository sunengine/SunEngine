using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SunEngine.Cli
{
    public static class InfoPrinter
    {
        /// <summary>
        /// Print this info if "dotnet SunEngine.dll" starts with no arguments.
        /// </summary>
        public static void PrintNoArgumentsInfo()
        {
            Console.WriteLine("Valid startup arguments wasn't provided. To list available commands use \"help\" argument.\n");
        }

        public static void PrintStart()
        {
            PrintSunEngineLogo();
            PrintVersion2();
            Console.WriteLine();
        }
        
        public static void PrintSunEngineLogo()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(@"
     ______             ______                         
    /  ____)           |  ____)            (_)           
   (  (___  _   _ ____ | |___   ____   ____ _ ____  _____ 
    \___  \| | | |  _ \|  ___) |  _ \ / _  | |  _ \| ___ |
    ____)  ) |_| | | | | |_____| | | ( (_| | | | | | ____|
   (______/|____/|_| |_|_______)_| |_|\___ |_|_| |_|_____)
                                     (_____|");
            
            Console.ResetColor();
        }
        
        public static void PrintVersion2()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fileVersionInfo.ProductVersion;
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"           Version: {version}");
            Console.ResetColor();
        }

        /// <summary>
        /// Print help on dotnet "dotnet SunEngine.dll help"
        /// </summary>
        public static void PrintHelp()
        {
            const int padding = -36;
            string helpText = $@"
Commands:
    {StartupConfiguration.ServerCommand,padding} host server api with kestrel
    {StartupConfiguration.ConfigArgumentName + ":<Path>",padding} path to config directory, if none ""Config"" is default, "".Config"" suffix at the end of the path can be skipped
    {StartupConfiguration.MigrateCommand,padding} make initial database table structure and migrations in existing database
    {StartupConfiguration.InitCommand,padding} initialize users, roles and categories tables from config directory
    {StartupConfiguration.TestDatabaseConnection,padding} check is data base connection is working                     
    {StartupConfiguration.VersionCommand,padding} print SunEngine version
    {StartupConfiguration.HelpCommand,padding} show this help   
    
Seed test data commands:    
    {StartupConfiguration.SeedCommand}:<CategoryName>:<MaterialsCount>:<CommentsCount>      
    {"",padding} seed category and all subcategories with materials and comments
    {"",padding} MaterialsCount and CommentsCount - default if skipped
    {"",padding} example - seed:SomeCategory:20:10
                                
    {StartupConfiguration.AppendCategoriesNamesCommand,padding} add category name to material titles on ""{StartupConfiguration.SeedCommand}""

Examples:
    dotnet SunEngine.dll {StartupConfiguration.ServerCommand}
    dotnet SunEngine.dll {StartupConfiguration.ServerCommand} {StartupConfiguration.ConfigArgumentName}:local.MySite
    dotnet SunEngine.dll {StartupConfiguration.MigrateCommand} {StartupConfiguration.InitCommand} {StartupConfiguration.SeedCommand}
    dotnet SunEngine.dll {StartupConfiguration.MigrateCommand} {StartupConfiguration.InitCommand} {StartupConfiguration.SeedCommand} {StartupConfiguration.ConfigArgumentName}:local.MySite
    dotnet SunEngine.dll {StartupConfiguration.SeedCommand}:Forum:10:10
";


            Console.WriteLine(helpText);
        }

        /// <summary>
        /// Print version of SunEngine
        /// </summary>
        public static void PrintVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fileVersionInfo.ProductVersion;
            Console.WriteLine($"SunEngine version: {version}");
        }
    }
}
