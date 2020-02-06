using System.IO;
using System.Text.Json;
using SunEngine.Core.Models;

namespace SunEngine.DataSeed
{
	public class ConfigurationItemsSeeder
	{
		protected readonly DataContainer dataContainer;
		protected readonly string configJsonPath;

		public ConfigurationItemsSeeder(DataContainer dataContainer, string configJsonPath)
		{
			this.dataContainer = dataContainer;
			this.configJsonPath = configJsonPath;
		}

		public void Seed()
		{
			var jDoc = JsonDocument.Parse(File.OpenRead(configJsonPath));
			foreach (var jProp in jDoc.RootElement.EnumerateObject())
			{
				dataContainer.ConfigurationItems.Add(
					new ConfigurationItem {
						Name = jProp.Name,
						Value = jProp.Value.ToString()
						}
					);
			}
		}
	}
}