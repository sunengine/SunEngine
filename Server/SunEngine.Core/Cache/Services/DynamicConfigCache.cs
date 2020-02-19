using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using SunEngine.Core.Configuration;

namespace SunEngine.Core.Cache.Services
{
	public interface IDynamicConfigCache
	{
		void Initialize();
		string DynamicConfigCacheJson { get; }
	}

	public class DynamicConfigCache : IDynamicConfigCache
	{
		protected IConfigurationRoot configurationRoot { get; }

		public string DynamicConfigCacheJson { get; private set; }

		public DynamicConfigCache(IConfigurationRoot configurationRoot)
		{
			this.configurationRoot = configurationRoot;
			Initialize();
		}

		public void Initialize()
		{
			var itemsToSaveDic = ConfigDefaults.ConfigurationItems.Where(x => x.Value.Dynamic)
				.ToDictionary(x => x.Key, x => x.Value);

			var rez = new Dictionary<string, object>();
			foreach (var (key, item) in itemsToSaveDic)
			{
				var value = configurationRoot.GetValue(item.ToClientType, key);

				string[] tokens = key.Split(":");

				Dictionary<string, object> current = rez;
				for (int i = 0; i < tokens.Length - 1; i++)
				{
					if (!current.ContainsKey(tokens[i]))
						current[tokens[i]] = new Dictionary<string, object>();

					current = (Dictionary<string, object>) current[tokens[i]];
				}

				current[tokens[^1]] = value;
			}
			
			DynamicConfigCacheJson = JsonSerializer.Serialize(rez, new JsonSerializerOptions
			{
				WriteIndented = true,
				AllowTrailingCommas = true,
			});
		}
	}
}