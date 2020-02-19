namespace SunEngine.Core.Configuration
{
	public class HtmlStringItem : ConfigItem
	{
		public HtmlStringItem(string value = "", bool configJs = false) : base(configJs)
		{
			StringValue = value;
		}
	}
}