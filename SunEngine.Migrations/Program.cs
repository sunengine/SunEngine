using System.Linq;

namespace Migrations
{
    class Program
    {
        static void Main(string[] args)
        {
            string configDir = args.FirstOrDefault(x => x.StartsWith("c:"));
            if (configDir != null)
                configDir = configDir.Substring(2);
            else
                configDir = "Config";
            
           MainMigrator mainMigrator = new MainMigrator(configDir);
           mainMigrator.Migrate();
        }
    }
}