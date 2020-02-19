using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.ConfigItemType;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Presenters
{
	public interface ISectionsAdminPresenter
	{
		Task<Section[]> GetSectionsAsync();
		Task<SectionView> GetSectionAsync(string name);
	}

	public class SectionsAdminPresenter : DbService, ISectionsAdminPresenter
	{
		protected ISectionsCache sectionsCache;

		public SectionsAdminPresenter(
			ISectionsCache sectionsCache,
			DataBaseConnection db) : base(db)
		{
			this.sectionsCache = sectionsCache;
		}

		public Task<Section[]> GetSectionsAsync()
		{
			return db.Sections.OrderBy(x => x.Name).ToArrayAsync();
		}

		public async Task<SectionView> GetSectionAsync(string name)
		{
			var section = await db.Sections.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower());
			var sectionView = new SectionView(section);

			var configItemViews = new Dictionary<string, ConfigItemView>();
			
			if (sectionsCache.SectionServerTypes.TryGetValue(section.Type, out Type sectionServerType))
				AddFields(sectionServerType);
			if (sectionsCache.SectionClientTypes.TryGetValue(section.Type, out Type sectionClientType))
				AddFields(sectionClientType);

			void AddFields(Type type)
			{
				var serverData = JsonSerializer.Deserialize(section.Options, type);
				var fields = type.GetFields();
				foreach (var field in fields)
				{
					ConfigItemAttribute configItemAttribute = field.GetCustomAttribute<ConfigItemAttribute>();
					var configItemView = new ConfigItemView();
					configItemView.Name = field.Name;
					configItemView.Type = configItemAttribute.ConfigItemType.Name.Split(".")[^1];
					configItemView.Value = field.GetValue(serverData);
					if (configItemView.Type == nameof(EnumItem))
						configItemView.Enum = field.FieldType.Name.Split(".")[^1];
					configItemViews[field.Name] = configItemView;
				}
			}

			sectionView.ConfigItems = configItemViews.Values.ToArray();
			
			return sectionView;
		}
	}

	public class SectionView
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Token { get; set; }
		public string Type { get; set; }
		public string Roles { get; set; }
		public bool IsCacheData { get; set; }
		public ConfigItemView[] ConfigItems { get; set; }

		public SectionView(Section section)
		{
			Id = section.Id;
			Name = section.Name;
			Token = section.Token;
			Type = section.Type;
			Roles = section.Roles;
			IsCacheData = section.IsCacheData;
		}
	}

	public class ConfigItemView
	{
		public string Name { get; set; }
		public object Value { get; set; }
		public string Type { get; set; }
		public string Enum { get; set; }
	}
}