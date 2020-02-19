namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class HtmlStringItem : ConfigItem
	{
		public HtmlStringItem(string value = "", bool configJs = false) : base(configJs)
		{
			StringValue = value;
		}
	}
}