using System;
using System.IO;
using System.Linq;
using AngleSharp.Extensions;
using AngleSharp.Parser.Html;
using Newtonsoft.Json.Linq;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Services;

namespace SunEngine.DataSeed
{
	public class MaterialsSeederFromDir
	{
		private readonly DataContainer dataContainer;
		private readonly string configDir;

		public MaterialsSeederFromDir(DataContainer dataContainer, string configDir)
		{
			this.configDir = configDir;
			this.dataContainer = dataContainer;
		}

		public void Seed()
		{
			var files = Directory.GetFiles(configDir);
			foreach (var file in files.Where(x => x.EndsWith(".json")))
			{
				string jsonText = File.ReadAllText(file);
				JObject jObject = JObject.Parse(jsonText);
				string html = File.ReadAllText(file.Replace(".json", ".html"));
				string name = Path.GetFileName(file);
				name = name.Substring(0, name.Length - 5);
				SeedMaterial(name, jObject, html);
			}
		}

		protected void SeedMaterial(string name, JObject jObject, string html)
		{
			Material material = new Material();
			material.Id = dataContainer.NextMaterialId();
			material.Name = name;
			material.Title = (string) jObject["Title"];

			if (jObject.TryGetValue("SubTitle", out JToken subTitle))
				material.SubTitle = (string) subTitle;
			if (jObject.TryGetValue("Author", out JToken authorJT))
			{
				User author = dataContainer.Users.FirstOrDefault(x => x.UserName == (string) authorJT);
				material.AuthorId = author.Id;
			}

			string categoryName = (string) jObject["Category"];
			var category = dataContainer.Categories.FirstOrDefault(x =>
				String.Equals(x.Name, categoryName, StringComparison.OrdinalIgnoreCase));
			material.CategoryId = category.Id;

			material.Text = new HtmlParser().Parse(html).Body.ChildNodes.ToHtml(Sanitizer.OutputFormatter);

			material.PublishDate = DateTime.UtcNow;
			material.SortNumber = material.Id;
			material.LastActivity = material.PublishDate;

			dataContainer.Materials.Add(material);
		}
	}
}