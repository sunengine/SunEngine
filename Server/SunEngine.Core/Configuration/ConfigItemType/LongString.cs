namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class LongString : ConfigItem
	{
		public LongString(string value = "", bool configJs = false) : base(configJs)
		{
			StringValue = value;
		}
	}
}