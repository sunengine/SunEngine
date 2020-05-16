using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using SunEngine.Core.Services;

namespace SunEngine.Cli
{
	/// <summary>
	/// Class to print SunEngine information
	/// </summary>
	public static class InfoPrinter
	{
		/// <summary>
		/// Print warning message if "dotnet SunEngine.dll" starts with no valid arguments.
		/// </summary>
		public static void PrintNoArgumentsInfo()
		{
			Console.WriteLine(
				"Valid startup arguments was not provided.\nTo list available arguments run with 'help' command.\n> dotnet SunEngine.dll help\n");
		}

		/// <summary>
		/// Print SunEngine logo 
		/// </summary>
		public static void PrintLogoAndVersion()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
			string v = fileVersionInfo.ProductVersion;
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine($@"
    _____                 _____
   /  ___)               |  ___)               (o)
  (  (___   _   _  ____  | |___   ____    ____  _  ____   ____ 
   \___  \ | | | ||  _ \ |  ___) |  _ \  / _  || ||  _ \ |  __)
   ____)  )| |_| || | | || |____ | | | |( (_| || || | | || |__)
  (______/ |_____/|_| |_||______)|_| |_| \__  ||_||_| |_||____)
                                          __| |
            Version: {v,-008}            (____/          ".TrimStart('\n') + "\n");

			Console.ResetColor();
		}

		/// <summary>
		/// Print help on dotnet "dotnet SunEngine.dll help"
		/// </summary>
		public static void PrintHelp()
		{
			const int padding = -36;
			string helpText = $@"
  Commands
     {StartupConfiguration.ServerCommand,padding} host server api with kestrel
     {StartupConfiguration.ConfigArgumentName + ":<path>",padding} path to config directory, if none ""Config"" is default, "".Config"" suffix at the end of the path can be skipped
     {StartupConfiguration.MigrateCommand,padding} make initial database table structure and migrations in existing database
     {StartupConfiguration.InitCommand,padding} initialize users, roles and categories tables from config directory
     {StartupConfiguration.TestDatabaseConnection,padding} check is data base connection is working                     
     {StartupConfiguration.VersionCommand,padding} print SunEngine version
     {StartupConfiguration.NoLogoCommand,padding} do not show logo image on start   
     {StartupConfiguration.HelpCommand,padding} show this help   
    
  Seed test data commands    
     {StartupConfiguration.SeedCommand}:<category>:<materials>:<comments>      
     {"",padding} seed category and all subcategories with materials and comments
     {"",padding} <category> - category name
     {"",padding} <materials> - materials count, default if skipped
     {"",padding} <comments> - comments count, default if skipped
     {"",padding} example - seed:SomeCategory:20:10
                                
     {StartupConfiguration.AppendCategoriesNamesCommand,padding} Add category name to material titles on ""{StartupConfiguration.SeedCommand}""

  Examples
     dotnet SunEngine.dll {StartupConfiguration.ServerCommand}
     dotnet SunEngine.dll {StartupConfiguration.ServerCommand} {StartupConfiguration.ConfigArgumentName}:local.MySite
     dotnet SunEngine.dll {StartupConfiguration.MigrateCommand} {StartupConfiguration.InitCommand} {StartupConfiguration.SeedCommand}
     dotnet SunEngine.dll {StartupConfiguration.MigrateCommand} {StartupConfiguration.InitCommand} {StartupConfiguration.SeedCommand} {StartupConfiguration.ConfigArgumentName}:local.MySite
     dotnet SunEngine.dll {StartupConfiguration.SeedCommand}:Forum:10:10
";

			Console.WriteLine(helpText);
		}

		/// <summary>
		/// Print SunEngine version
		/// </summary>
		public static void PrintVersion()
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
			string version = fileVersionInfo.ProductVersion;
			Console.WriteLine($"SunEngine version: {version}");
		}

		public static void PrintServerInfo(string configRootDir)
		{
			PathService pathService = new PathService(configRootDir);
			var serverInfoJsonPath = pathService.Combine(PathNames.ConfigDirName, PathNames.ServerInfoJsonFileName);
			try
			{
				var jsonText = File.ReadAllText(serverInfoJsonPath);
				var serverInfo = JsonDocument.Parse(jsonText).RootElement.GetProperty("ServerInfo");
				var serverName = serverInfo.GetProperty("Name").GetString();
				var serverVersion = serverInfo.GetProperty("ServerVersion").GetString();
				Console.WriteLine($"Server name: {serverName}");
				if (serverVersion != null)
					Console.WriteLine($"Server version: {serverVersion}");
			}
			catch (Exception)
			{
				Console.WriteLine($"No server info available at: " + serverInfoJsonPath);
			}
		}
	}
}