using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Reflection;

namespace SunEngine
{
    public static class InfoPrinter
    { 
        public static void PrintHelp()
        {
            var text = File.ReadAllText(Path.GetFullPath("Resources/help.txt"));
            Console.WriteLine(text);
        }

        public static void PrintVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = fileVersionInfo.ProductVersion;
            Console.WriteLine("SunEngine Version: " + version);
        }
    }
}