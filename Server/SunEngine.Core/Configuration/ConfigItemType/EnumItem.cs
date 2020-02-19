using System;

namespace SunEngine.Core.Configuration
{
	public class EnumItem : ConfigItem
	{
		public Enum Enum { get; }

		public EnumItem(Enum value, bool jsConfig = false) : base(jsConfig)
		{
			Enum = value;
			_objectValue = value;
			_stringValue = value.ToString();
		}

		public EnumItem(Type type, string value, bool jsConfig = false) : base(jsConfig)
		{
			Enum = Enum.Parse(type, value, true) as Enum;
			_objectValue = value;
			_stringValue = value.ToString();
		}
	}
}