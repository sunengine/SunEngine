using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
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
		Dictionary<string, IEnumerable<string>> GetEnums();
	}


	public class ConfigurationPresenter : DbService, IConfigurationPresenter
	{
		public ConfigurationPresenter(DataBaseConnection db) : base(db)
		{
		}

		public async Task<IEnumerable<ConfigurationItemView>> LoadConfigurationAsync()
		{
			var items = await db.ConfigurationItems.OrderBy(x => x.Name).ToDictionaryAsync(x => x.Name, x => x.Value);
			var order = ConfigDefaults.ConfigurationItems.Select(item => item.Key).ToList();

			var rez = new List<ConfigurationItemView>();

			foreach (var name in order)
				if (items.TryGetValue(name, out string value))
					rez.Add(new ConfigurationItemView(name, value));

			return rez;
		}

		public Dictionary<string, IEnumerable<string>> GetEnums()
		{
			var enums = new Dictionary<string, Type>();

			foreach (var configurationItem in ConfigDefaults.ConfigurationItems)
			{
				var type = configurationItem.Value.GetType();
				if (type.IsEnum && !enums.ContainsKey(type.Name))
					enums[type.Name] = type;
			}

			var rez = new Dictionary<string, IEnumerable<string>>();
			foreach (var (name, enum1) in enums)
			{
				rez.Add(name, enum1.GetEnumNames());
			}

			return rez;
		}
	}
}