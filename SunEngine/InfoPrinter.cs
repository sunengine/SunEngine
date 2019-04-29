using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace SunEngine
{
    public class InfoPrinter
    { 
        /// <summary>
        /// Print this info if "dotnet SunEngine.dll" starts with no commands.
        /// </summary>
        public void PrintVoidStartInfo()
        {
            Console.WriteLine("You start \"SunEngine.dll\" with out arguments.\nRun with \"help\" argument to list commands:\n\tdotnet SunEngine.dll help");
        }
        
        /// <summary>
        /// Print help on dotnet "dotnet SunEngine.dll help"
        /// </summary>
        public void PrintHelp()
        {
            var text = File.ReadAllText(Path.GetFullPath("Resources/help.txt"));
            Console.WriteLine(text);
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