using System.IO;
using System.Linq;

namespace SunEngine.DataSeed
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string configDir = args.FirstOrDefault(x => x.StartsWith("config:"));
            if (configDir != null)
                configDir = configDir.Substring("config:".Length);
            else
                configDir = "Config";

            configDir = Path.GetFullPath(configDir);
            
            if (args.Any(x => x == "init"))
            {
                MainSeeder ms = new MainSeeder(configDir);
                ms.SeedInitialize();
            }
            if (args.Any(x => x == "add-test-data"))
            {
                MainSeeder ms = new MainSeeder(configDir);
                ms.SeedAddTestData();
            }
        }
    }
}