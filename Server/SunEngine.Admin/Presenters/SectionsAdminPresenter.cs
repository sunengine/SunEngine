using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Admin.Presenters.ViewModels;
using SunEngine.Core.Configuration.ConfigItemType;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Security;
using SunEngine.Core.Services;

namespace SunEngine.Admin.Presenters
{
	public interface ISectionsAdminPresenter
	{
		Task<Section[]> GetSectionsAsync();
		Task<SectionView> GetSectionAsync(string name);
		SectionView GetSectionTemplate(string templateName);
	}

	public class SectionsAdminPresenter : DbService, ISectionsAdminPresenter
	{
		protected SectionTypes sectionTypes;

		public SectionsAdminPresenter(
			SectionTypes sectionTypes,
			DataBaseConnection db) : base(db)
		{
			this.sectionTypes = sectionTypes;
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

			if (sectionTypes.Sections.TryGetValue(section.Type, out SectionDescriptor sectionDescriptor))
			{
				sectionView.GroupName = sectionDescriptor.SectionGroup;
				AddFields(sectionDescriptor.ClientSectionType);
				AddFields(sectionDescriptor.ServerSectionType);
			}
			
			sectionView.Options = configItemViews.Values.ToArray();
			return sectionView;
			
			void AddFields(Type sectionType)
			{
				var dataObject = JsonSerializer.Deserialize(section.Options, sectionType);

				foreach (var propertyInfo in sectionType.GetProperties())
				{
					ConfigItemAttribute configItemAttribute = propertyInfo.GetCustomAttribute<ConfigItemAttribute>();
					var configItemView = new ConfigItemView
					{
						Name = propertyInfo.Name,
						Type = configItemAttribute.ConfigItemType.Name.Split(".")[^1].Replace("Item", "")
					};

					object value = propertyInfo.GetValue(dataObject);
					Type type = propertyInfo.PropertyType;

					IConfigItem configItem;
					if (configItemView.Type == "Enum")
					{
						configItem = new EnumItem((Enum) value);
						configItemView.Enum = propertyInfo.PropertyType.Name.Split(".")[^1];
						if (!sectionView.Enums.ContainsKey(configItemView.Enum))
							sectionView.Enums.Add(configItemView.Enum, propertyInfo.PropertyType.GetEnumNames());
					}
					else
						configItem = (IConfigItem) configItemAttribute.ConfigItemType.GetConstructor(new[] {type})
							.Invoke(new[] {value});

					configItemView.Value = configItem.ToClientObject();

					configItemViews[propertyInfo.Name] = configItemView;
				}
			}
		}

		public SectionView GetSectionTemplate(string templateName)
		{
			var sectionView = new SectionView();

			var configItemViews = new Dictionary<string, ConfigItemView>();

			sectionView.Type = templateName;
			sectionView.Roles = string.Join(",", RoleNames.Unregistered, RoleNames.Registered);
			sectionView.Enums = new Dictionary<string, string[]>();
			
			if (sectionTypes.Sections.TryGetValue(templateName, out SectionDescriptor sectionDescriptor))
			{
				AddFields(sectionDescriptor.ServerSectionType);
				AddFields(sectionDescriptor.ClientSectionType);
				sectionView.GroupName = sectionDescriptor.SectionGroup;
			}
			
			sectionView.Options = configItemViews.Values.ToArray();
			return sectionView;
			
			void AddFields(Type sectionType)
			{
				var section = sectionType.GetConstructor(new Type[0]).Invoke(new object[0]);

				foreach (var propertyInfo in sectionType.GetProperties())
				{
					ConfigItemAttribute configItemAttribute = propertyInfo.GetCustomAttribute<ConfigItemAttribute>();
					ConfigItemView configItemView = new ConfigItemView()
					{
						Name = propertyInfo.Name,
						Value = propertyInfo.GetValue(section),
						Type = configItemAttribute.ConfigItemType.Name.Split(".")[^1].Replace("Item", "")
					};
					if (configItemView.Type == "Enum")
					{
						configItemView.Enum = propertyInfo.PropertyType.Name.Split(".")[^1];
						if (!sectionView.Enums.ContainsKey(configItemView.Enum))
							sectionView.Enums.Add(configItemView.Enum, propertyInfo.PropertyType.GetEnumNames());
					}

					configItemViews[configItemView.Name] = configItemView;
				}
			}

			
		}
	}
}