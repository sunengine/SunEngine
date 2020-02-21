namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class StringItem : ConfigItem<string>
	{
		public StringItem(string value = "", bool configJs = false) : base(value, configJs)
		{
		}

		public override void FromString(string value)
		{
			value = value;
		}
	}
}