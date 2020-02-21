using System;

namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class IntegerItem : ConfigItem<int>
	{
		public IntegerItem(int value = 0, bool configJs = false) : base(value, configJs)
		{
		}

		public IntegerItem(string value, bool configJs = false) : base(int.Parse(value), configJs)
		{
		}
		
		public override void FromString(string value)
		{
			Value = int.Parse(value);
		}
	}
}