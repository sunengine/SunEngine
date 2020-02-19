using System;

namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class EnumItem : ConfigItem
	{
		public Enum Enum { get; }

		public override Type ToClientType => typeof(string);

		public EnumItem(Enum value, bool configJs = false) : base(configJs)
		{
			Enum = value;
			_objectValue = value;
			_stringValue = value.ToString();
		}

		public EnumItem(Type type, string value, bool configJs = false) : base(configJs)
		{
			Enum = Enum.Parse(type, value, true) as Enum;
			_objectValue = value;
			_stringValue = value.ToString();
		}
	}
}