using System;

namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class EnumItem : ConfigItem<Enum>
	{
		public Type Type { get; }
		public override Type ToClientType() => typeof(string);
		public override object ToClientObject() => Value.ToString();
		
		public EnumItem(Enum value) : base(value)
		{
			Type = value.GetType();
		}

		public EnumItem(Type type, string value) : base((Enum) Enum.Parse(type, value, true))
		{
			Type = type;
		}
		
		public EnumItem(Enum value, bool configJs) : base(value, configJs)
		{
			Type = value.GetType();
		}

		public EnumItem(Type type, string value, bool configJs) : base((Enum) Enum.Parse(type, value, true),
			configJs)
		{
			Type = type;
		}
		
		public override void FromString(string value)
		{
			Value = (Enum)Enum.Parse(Type, value);
		}
	}
}