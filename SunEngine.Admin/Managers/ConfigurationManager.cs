using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using SunEngine.Core.Configuration;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Managers
{
    public interface IConfigurationManager
    {
        void UploadConfigurationItems(IList<ConfigurationItem> configurationItems);
    }

    public class ConfigurationManager : DbService, IConfigurationManager
    {
        public ConfigurationManager(DataBaseConnection db) : base(db)
        {
        }

        public void UploadConfigurationItems(IList<ConfigurationItem> configurationItems)
        {
            List<ConfigurationItem> itemsToUpdate = new List<ConfigurationItem>();
            
            foreach (var configurationItem in configurationItems)
            {
                if (!ConfigDefaults.ConfigurationItems.TryGetValue(configurationItem.Name,out object value))
                    continue;

                
                switch (value.GetType().Name.Split(".")[^1])
                {
                    case "Int64":
                    case "Int32":
                    case "int":
                        if(!int.TryParse(configurationItem.Value,out _))
                            continue;
                        itemsToUpdate.Add(configurationItem);
                        break;
                    case "Boolean":
                    case "bool":
                        if(!bool.TryParse(configurationItem.Value,out _))
                            continue;
                        itemsToUpdate.Add(configurationItem);
                        break;
                    case "String":
                    case "string":
                        itemsToUpdate.Add(configurationItem);
                        break;
                }
            }

            db.ConfigurationItems.BulkCopy(itemsToUpdate);
        }
    }
}