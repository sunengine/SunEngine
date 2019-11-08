using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.Configuration;
using SunEngine.Core.DataBase;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Presenters
{
    public interface IConfigurationPresenter
    {
        Task<IEnumerable<ConfigurationItemView>> LoadConfigurationAsync();
    }


    
    public class ConfigurationPresenter : DbService, IConfigurationPresenter
    {
        public ConfigurationPresenter(DataBaseConnection db) : base(db)
        {
        }

        public async Task<IEnumerable<ConfigurationItemView>> LoadConfigurationAsync()
        {
            var items = await db.ConfigurationItems.OrderBy(x => x.Name).ToListAsync();
            return items.Select(x=>new ConfigurationItemView
            {
                Name = x.Name,
                Value = x.Value,
                Type = ConfigDefaults.ConfigurationItems.ContainsKey(x.Name) 
                    ? GetTypeName(ConfigDefaults.ConfigurationItems[x.Name].GetType().Name)
                    : null 
            }).ToList();
        }

        protected string GetTypeName(string name)
        {
            var nameLastToken = name.Split(".")[^1];
            switch (nameLastToken)
            {
                case "Int64":
                case "Int32":
                case "int":
                    return "number";
                case "Boolean":
                case "bool":
                    return "boolean";
                case "String":
                case "string":
                    return "string";
                default:
                    return nameLastToken.ToLower();
            }
        }
    }
    
    public class ConfigurationItemView
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Type { get; set; }
    }
}