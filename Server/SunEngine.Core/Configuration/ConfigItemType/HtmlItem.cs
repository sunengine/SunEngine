namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class HtmlItem : StringItem
	{
		public HtmlItem(string value = "") : base(value)
		{
		}
		
		public HtmlItem(string value, bool configJs) : base(value, configJs)
		{
		}
	}
}