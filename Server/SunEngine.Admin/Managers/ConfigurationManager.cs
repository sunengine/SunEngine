using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB.Data;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration;
using SunEngine.Core.Configuration.ConfigItemType;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Managers
{
	public interface IConfigurationManager
	{
		void UploadConfigurationItems(IEnumerable<ConfigurationItem> configurationItems);
	}

	public class ConfigurationManager : DbService, IConfigurationManager
	{
		protected readonly SanitizerService sanitizerService;
		protected readonly ICategoriesCache categoriesCache;

		public ConfigurationManager(
			SanitizerService sanitizerService,
			ICategoriesCache categoriesCache,
			DataBaseConnection db) : base(db)
		{
			this.sanitizerService = sanitizerService;
			this.categoriesCache = categoriesCache;
		}

		public void UploadConfigurationItems(IEnumerable<ConfigurationItem> configurationItems)
		{
			Dictionary<string, string> allItems = db.ConfigurationItems.ToDictionary(x => x.Name, x => x.Value);
			List<ConfigurationItem> itemsToUpdate = new List<ConfigurationItem>();

			var enums = new Dictionary<string, Type>();

			foreach (var configurationItem in ConfigDefaults.ConfigurationItems)
			{
				var type = configurationItem.Value.GetType();
				if (type.IsEnum && !enums.ContainsKey(type.Name))
					enums[type.Name] = type;
			}

			foreach (var configurationItem in configurationItems)
			{
				configurationItem.Value = configurationItem.Value?.Trim();

				if (!ConfigDefaults.ConfigurationItems.TryGetValue(configurationItem.Name, out ConfigItem defaultConfigItem)
				)
					continue;

				var configItem = defaultConfigItem.ShallowCopy();
				configItem.StringValue = configurationItem.Value;

				if (string.Equals(allItems[configurationItem.Name], configurationItem.Value,
					StringComparison.OrdinalIgnoreCase))
					return;

				itemsToUpdate.Add(configurationItem);
			}

			db.BulkCopy( new BulkCopyOptions {BulkCopyType = BulkCopyType.MultipleRows  },itemsToUpdate);
			
			foreach (var configurationItem in itemsToUpdate)
				ItemDo(configurationItem);
		}

		protected void ItemDo(ConfigurationItem item)
		{
			switch (item.Name)
			{
				case "Global:ReadOnlyMode":
					categoriesCache.Initialize();
					return;
			}
		}
	}
}