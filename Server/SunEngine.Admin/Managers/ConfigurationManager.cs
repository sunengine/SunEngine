using System;
using System.Collections.Generic;
using System.Linq;
using LinqToDB;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

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

				if (!ConfigDefaults.ConfigurationItems.TryGetValue(configurationItem.Name, out object value))
					continue;

				if (value.GetType().IsEnum)
				{
					var enumType = value.GetType();
					if (Enum.TryParse(enumType, configurationItem.Value, out _))
						TryAdd();

					continue;
				}

				switch (value.GetType().Name.Split(".")[^1])
				{
					case "Int64":
					case "Int32":
					case "int":
						if (!int.TryParse(configurationItem.Value, out _))
							continue;
						TryAdd();
						break;
					case "Boolean":
					case "bool":
						if (!bool.TryParse(configurationItem.Value, out _))
							continue;
						TryAdd();
						break;
					case "LongString":
						TryAdd();
						break;
					case "JsonString":
						configurationItem.Value = configurationItem.Value.MakeJsonText();
						TryAdd();
						break;
					case "HtmlString":
						configurationItem.Value = sanitizerService.Sanitize(configurationItem.Value) ?? String.Empty;
						TryAdd();
						break;
					case "String":
					case "string":
						TryAdd();
						break;
				}

				void TryAdd()
				{
					if (string.Equals(allItems[configurationItem.Name], configurationItem.Value,
						StringComparison.OrdinalIgnoreCase))
						return;

					itemsToUpdate.Add(configurationItem);
				}
			}

			foreach (var configurationItem in itemsToUpdate)
			{
				db.ConfigurationItems.Where(x => x.Name == configurationItem.Name)
					.Set(x => x.Value, configurationItem.Value).Update();
				ItemDo(configurationItem);
			}
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