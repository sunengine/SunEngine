using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.ConfigItemType;
using SunEngine.Core.Models;

namespace SunEngine.Core.Utils
{
	public static class SectionsUtils
	{
		public static void SectionProcess(Section section, SectionTypes sectionTypes, ICheckRoles checkRoles)
		{
			section.Options = section.Options?.MakeJsonTextNotNull();

			if (checkRoles != null)
				section.Roles = checkRoles.CheckAndSetRoles(section.Roles);

			var options = new Dictionary<string, object>();

			if (sectionTypes.SectionServerTypes.TryGetValue(section.Type, out Type sectionServerType))
				AddFields(section.Options, sectionServerType, options);
			if (sectionTypes.SectionClientTypes.TryGetValue(section.Type, out Type sectionClientType))
				AddFields(section.Options, sectionClientType, options);

			section.Options = JsonSerializer.Serialize(options);

			void AddFields(string optionsJson, Type sectionType, Dictionary<string, object> optionsToAdd)
			{
				var dataObject = JsonSerializer.Deserialize(optionsJson, sectionType);
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

					optionsToAdd[name] = configItem.ToClientObject();
				}
			}

			section.Options = JsonSerializer.Serialize(options);
		}
	}
}