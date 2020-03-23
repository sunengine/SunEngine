using System.IO;
using Newtonsoft.Json.Linq;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Models;
using SunEngine.Core.Security;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

namespace SunEngine.DataSeed
{
	public class SectionsSeeder
	{
		private readonly DataContainer dataContainer;
		private readonly string configDir;
		protected readonly SectionTypes sectionTypes;

		public SectionsSeeder(DataContainer dataContainer, string configDir)
		{
			this.configDir = configDir;
			this.dataContainer = dataContainer;
			sectionTypes = new SectionTypes();
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

			if (jComponent.TryGetValue("IsCacheData", out JToken isCacheData) && (bool) isCacheData)
				section.IsCacheData = true;

			if (jComponent.TryGetValue("Token", out JToken token))
				section.Token = (string) token;

			if (jComponent.TryGetValue("Options", out JToken serverSettingsJson))
				section.Options = serverSettingsJson.ToString();
			else
				section.Options = "{}";

			if (jComponent.TryGetValue("Roles", out JToken roles))
				section.Roles = (string) roles;
			else
				section.Roles = string.Join(',', RoleNames.Unregistered, RoleNames.Registered);

			SectionsUtils.SectionProcess(section, sectionTypes, null);

			dataContainer.Sections.Add(section);
		}
	}
}