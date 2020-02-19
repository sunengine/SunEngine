using System;

namespace SunEngine.Core.Configuration
{
	public class HtmlStringItem : ConfigItem
	{
		public HtmlStringItem(string value = "", bool jsConfig = false) : base(jsConfig)
		{
			StringValue = value;
		}
	}
}