using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Presenters;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.ConfigItemType;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

namespace SunEngine.Admin.Managers
{
	public interface ISectionsAdminManager
	{
		Task CreateSectionAsync(Section section);
		Task UpdateSectionAsync(Section section);
		Task DeleteSectionAsync(int sectionId);
	}

	public class SectionsAdminManager : DbService, ISectionsAdminManager
	{
		protected readonly IRolesCache rolesCache;
		protected readonly ISectionsCache sectionsCache;

		public SectionsAdminManager(
			IRolesCache rolesCache,
			ISectionsCache sectionsCache,
			DataBaseConnection db) : base(db)
		{
			this.sectionsCache = sectionsCache;
			this.rolesCache = rolesCache;
		}

		public Task CreateSectionAsync(Section section)
		{
			section.Options = section.Options?.MakeJsonTextNotNull();
			section.Roles = rolesCache.CheckAndSetRoles(section.Roles);

			return db.InsertWithIdentityAsync(section);
		}

		public Task UpdateSectionAsync(Section section)
		{
			section.Options = section.Options?.MakeJsonTextNotNull();
			section.Roles = rolesCache.CheckAndSetRoles(section.Roles);

			var options = new Dictionary<string, object>();
			
			if (sectionsCache.SectionServerTypes.TryGetValue(section.Type, out Type sectionServerType))
				AddFields(sectionServerType);
			if (sectionsCache.SectionClientTypes.TryGetValue(section.Type, out Type sectionClientType))
				AddFields(sectionClientType);

			section.Options = JsonSerializer.Serialize(options);
			
			return db.UpdateAsync(section);
			
			void AddFields(Type sectionType)
			{
				var dataObject = JsonSerializer.Deserialize(section.Options, sectionType);
				var properties = sectionType.GetProperties();
				foreach (var propertyInfo in properties)
				{
					ConfigItemAttribute configItemAttribute = propertyInfo.GetCustomAttribute<ConfigItemAttribute>();

					string name = propertyInfo.Name;
					string typeName = configItemAttribute.ConfigItemType.Name.Split(".")[^1].Replace("Item", "");
					object value = propertyInfo.GetValue(dataObject);
					Type type = propertyInfo.PropertyType;

					IConfigItem configItem;
					if (typeName == "Enum")
						configItem = new EnumItem((Enum) value);
					else
						configItem = (IConfigItem) configItemAttribute.ConfigItemType.GetConstructor(new[] {type})
							.Invoke(new[] {value});
					
					options[name] = configItem.ToClientObject();
				}
			}
		}


		public Task DeleteSectionAsync(int sectionId)
		{
			return db.Sections.Where(x => x.Id == sectionId).DeleteAsync();
		}
	}
}