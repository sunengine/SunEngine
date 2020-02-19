using System;

namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class ConfigItemAttribute : Attribute
	{
		public Type ConfigItemType { get; set; }

		public ConfigItemAttribute(Type configItemType)
		{
			this.ConfigItemType = configItemType;
		}
	}
}