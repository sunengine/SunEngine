namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class StringItem : ConfigItem<string>
	{
		public StringItem(string value = "") : base(value)
		{
		}
		
		public StringItem(string value, bool configJs) : base(value ?? "", configJs)
		{
		}

		public override void FromString(string value)
		{
			value = value;
		}
	}
}