using System;

namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class BooleanItem : ConfigItem<bool>
	{
		public BooleanItem(bool value = false) : base(value, false)
		{
		}

		public BooleanItem(string value) : base(bool.Parse(value), false)
		{
		}
		
		public BooleanItem(bool value, bool configJs) : base(value, configJs)
		{
		}

		public BooleanItem(string value, bool configJs) : base(bool.Parse(value), configJs)
		{
		}

		public override void FromString(string value)
		{
			Value = bool.Parse(value);
		}
	}
}