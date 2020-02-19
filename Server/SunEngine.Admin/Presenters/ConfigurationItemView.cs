using SunEngine.Core.Configuration;
using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Admin.Presenters
{
	public class ConfigurationItemView
	{
		public string Name { get; set; }
		public object Value { get; set; }
		public string Type { get; set; }
		public string EnumName { get; set; }

		public ConfigurationItemView(string name, string value)
		{
			Name = name;

			if (!ConfigDefaults.ConfigurationItems.TryGetValue(name, out ConfigItem configurationItemDefault))
				return;

			var configurationItem = configurationItemDefault.ShallowCopy();
			configurationItem.StringValue = value;

			Value = configurationItem.ObjectValue;
			Type = configurationItem.GetTypeName();
			if (Type == nameof(EnumItem))
				EnumName = configurationItem.ObjectValue.GetType().Name.Split(".")[^1];
		}
	}
}