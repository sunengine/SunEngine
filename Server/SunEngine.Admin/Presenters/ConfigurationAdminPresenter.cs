using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AngleSharp.Common;
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
    private readonly IPathService pathService;
		public ConfigurationAdminPresenter(DataBaseConnection db,
      IPathService pathService) : base(db)
    {
      this.pathService = pathService;
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

      using FileStream fs = new FileStream(Path.GetFullPath(pathService.Combine(PathNames.ConfigDirName, PathNames.CustomConfigJsonFileName)), FileMode.Open);
      Dictionary<string, string> customConfigs = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(fs);
      
      foreach (KeyValuePair<string, string> valuePair in customConfigs)
      {
        rez.ConfigItems.Add(new ConfigItemView()
        {
          Name = valuePair.Key,
          Value = valuePair.Value
        });
      }
      
			return rez;
		}
	}
}
