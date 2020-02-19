using System.IO;
using Newtonsoft.Json.Linq;
using SunEngine.Core.Models;
using SunEngine.Core.Security;

namespace SunEngine.DataSeed
{
	public class SectionsSeeder
	{
		private readonly DataContainer dataContainer;
		private readonly string configDir;

		public SectionsSeeder(DataContainer dataContainer, string configDir)
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
			Section section = new Section
			{
				Id = dataContainer.NextComponentId(),
				Name = (string) jComponent["Name"],
				Type = (string) jComponent["Type"]
			};

			if (jComponent.TryGetValue("IsCacheData", out JToken value) && (bool) value)
				section.IsCacheData = true;

			if (jComponent.TryGetValue("ServerSettingsJson", out JToken serverSettingsJson))
				section.Options = serverSettingsJson.ToString();
			else
				section.Options = "{}";

			if (jComponent.TryGetValue("Roles", out JToken roles))
				section.Roles = (string) roles;
			else
				section.Roles = string.Join(',', RoleNames.Unregistered, RoleNames.Registered);
			
			dataContainer.Sections.Add(section);
		}
	}
}