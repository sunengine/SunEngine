using System.IO;
using Newtonsoft.Json.Linq;
using SunEngine.Core.Models;
using SunEngine.Core.Security;

namespace SunEngine.DataSeed
{
	public class ComponentsSeeder
	{
		private readonly DataContainer dataContainer;
		private readonly string configDir;

		public ComponentsSeeder(DataContainer dataContainer, string configDir)
		{
			this.configDir = configDir;
			this.dataContainer = dataContainer;
		}

		public void Seed()
		{
			var files = Directory.GetFiles(configDir);
			foreach (var file in files)
			{
				string jsonText = File.ReadAllText(file);
				JObject jObject = JObject.Parse(jsonText);
				SeedComponent(jObject);
			}
		}

		protected void SeedComponent(JObject jComponent)
		{
			Component component = new Component
			{
				Id = dataContainer.NextComponentId(),
				Name = (string) jComponent["Name"],
				Type = (string) jComponent["Type"]
			};

			if (jComponent.TryGetValue("IsCacheData", out JToken value) && (bool) value)
				component.IsCacheData = true;

			if (jComponent.TryGetValue("ServerSettingsJson", out JToken serverSettingsJson))
				component.ServerSettingsJson = serverSettingsJson.ToString();
			else
				component.ServerSettingsJson = VoidJson();

			if (jComponent.TryGetValue("ClientSettingsJson", out JToken clientSettingsJson))
				component.ClientSettingsJson = clientSettingsJson.ToString();
			else
				component.ClientSettingsJson = VoidJson();

			if (jComponent.TryGetValue("Roles", out JToken roles))
				component.Roles = (string) roles;
			else
				component.Roles = string.Join(',', RoleNames.Unregistered, RoleNames.Registered);


			dataContainer.Components.Add(component);

			string VoidJson()
			{
				return "{}";
			}
		}
	}
}