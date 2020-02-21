using System;

namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class BooleanItem : ConfigItem<bool>
	{
		public BooleanItem(bool value = false, bool configJs = false) : base(value, configJs)
		{
		}

		public BooleanItem(string value, bool configJs = false) : base(bool.Parse(value), configJs)
		{
		}

		public override void FromString(string value)
		{
			Value = bool.Parse(value);
		}
	}
}