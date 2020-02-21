using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Admin.Presenters.ViewModels;
using SunEngine.Core.Configuration;
using SunEngine.Core.Configuration.ConfigItemType;
using SunEngine.Core.DataBase;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Presenters
{
	public interface IConfigurationAdminPresenter
	{
		Task<ConfigurationView> LoadConfigurationAsync();
	}
	
	public class ConfigurationAdminPresenter : DbService, IConfigurationAdminPresenter
	{
		public ConfigurationAdminPresenter(DataBaseConnection db) : base(db)
		{
		}

		public async Task<ConfigurationView> LoadConfigurationAsync()
		{
			var items = await db.ConfigurationItems.OrderBy(x => x.Name).ToDictionaryAsync(x => x.Name, x => x.Value);
			var order = ConfigDefaults.ConfigurationItems.Select(item => item.Key).ToList();

			ConfigurationView rez = new ConfigurationView();

			foreach (var name in order)
				if (items.TryGetValue(name, out string value))
				{
					var configItem = ConfigDefaults.ConfigurationItems[name].ShallowCopy();
					configItem.FromString(value);

					var configItemView = new ConfigItemView
					{
						Name = name,
						Value = configItem.ToClientObject(),
						Type = configItem.GetType().Name.Split(".")[^1].Replace("Item", "")
					};
					
					if (configItemView.Type == "Enum")
					{
						configItemView.Enum = ((EnumItem) configItem).Value.GetType().Name.Split(".")[^1];  
						if (!rez.Enums.ContainsKey(configItemView.Enum))
							rez.Enums.Add(configItemView.Enum,  ((EnumItem) configItem).Value.GetType().GetEnumNames());
					}

					rez.ConfigItems.Add(configItemView);
				}

			
			return rez;
		}
	}
}