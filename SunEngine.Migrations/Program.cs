using System.IO;
using System.Linq;

namespace Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            string configDir = args.FirstOrDefault(x => x.StartsWith("config:"));
            if (configDir != null)
                configDir = configDir.Substring("config:".Length);
            else
                configDir = "Config";

            configDir = Path.GetFullPath(configDir);
            
           MainMigrator mainMigrator = new MainMigrator(configDir);
           mainMigrator.Migrate();
        }
    }
}