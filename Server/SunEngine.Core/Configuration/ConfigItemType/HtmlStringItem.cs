namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class HtmlStringItem : StringItem
	{
		public HtmlStringItem(string value = "") : base(value)
		{
		}
		
		public HtmlStringItem(string value, bool configJs) : base(value, configJs)
		{
		}
	}
}