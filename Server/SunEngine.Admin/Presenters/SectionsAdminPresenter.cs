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

			sectionView.Enums = new Dictionary<string, string[]>();

			if (sectionsCache.SectionServerTypes.TryGetValue(section.Type, out Type sectionServerType))
				AddFields(sectionServerType);
			if (sectionsCache.SectionClientTypes.TryGetValue(section.Type, out Type sectionClientType))
				AddFields(sectionClientType);

			void AddFields(Type type)
			{
				var dataObject = JsonSerializer.Deserialize(section.Options, type);
				var properties = type.GetProperties();
				foreach (var propertyInfo in properties)
				{
					ConfigItemAttribute configItemAttribute = propertyInfo.GetCustomAttribute<ConfigItemAttribute>();
					var configItemView = new ConfigItemView
					{
						Name = propertyInfo.Name,
						Type = configItemAttribute.ConfigItemType.Name.Split(".")[^1].Replace("Item", "")
					};
					
					object value = propertyInfo.GetValue(dataObject);
					Type t = propertyInfo.PropertyType;

					IConfigItem configItem;
					if (configItemView.Type == "Enum")
					{
						configItem = new EnumItem((Enum)value);
						configItemView.Value = configItem.ToClientObject();
						
						configItemView.Enum = propertyInfo.PropertyType.Name.Split(".")[^1];
						if (!sectionView.Enums.ContainsKey(configItemView.Enum))
							sectionView.Enums.Add(configItemView.Enum, propertyInfo.PropertyType.GetEnumNames());
					}
					else
						configItem = (IConfigItem) configItemAttribute.ConfigItemType.GetConstructor(new[] {t})
							.Invoke(new [] {value});
					
					configItemView.Value = configItem.ToClientObject();

					configItemViews[propertyInfo.Name] = configItemView;
				}
			}

			sectionView.ConfigItems = configItemViews.Values.ToArray();
			return sectionView;
		}
	}
}