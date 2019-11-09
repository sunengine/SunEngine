using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.Extensions.Configuration;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;

namespace SunEngine.Core.Configuration
{
    public class ConfigDbProvider : ConfigurationProvider
    {
        public static ConfigDbProvider DefaultConfigDbProvider;
        
        protected readonly IDataBaseFactory dataBaseFactory;

        public ConfigDbProvider(IDataBaseFactory dataBaseFactory)
        {
            this.dataBaseFactory = dataBaseFactory;
        }

        public override void Load()
        {
            Data.Clear();

            using var db = dataBaseFactory.CreateDb();

            EnsureItems(db);

            var items = db.ConfigurationItems.ToList();

            foreach (var item in items)
                Data.Add(item.Name, item.Value);
        }


        private static void EnsureItems(DataBaseConnection db)
        {
            var allItemsDic = db.ConfigurationItems.ToList().ToDictionary(x => x.Name, x => x);
            var itemsToAdd = new List<ConfigurationItem>();

            foreach (var (name, value) in ConfigDefaults.ConfigurationItems)
                if (!allItemsDic.ContainsKey(name))
                    itemsToAdd.Add(
                        new ConfigurationItem
                        {
                            Name = name,
                            Value = value.ToString()
                        }
                    );

            db.BulkCopy(itemsToAdd);
        }
    }
}