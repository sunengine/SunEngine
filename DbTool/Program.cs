using System;
using System.IO;
using System.Linq;

namespace DbTool
{
    class Program
    {
        private static string configFolder;
        private static string configFolderPath;
        
        static void Main(string[] args)
        {
            configFolder = args.FirstOrDefault(x => x.StartsWith("c:")) ?? "Config";
            configFolderPath = Path.GetFullPath(configFolder);
            
            if(args.Any(x=>x == "migrate"))
                
        }

        //private static string[] commandsOrdered = { "migrate", "init", "seed-test" };

    }
}