using System;

namespace SunEngine.Core.Configuration
{
	public class HtmlStringItem : ConfigItem
	{
		public static implicit operator HtmlStringItem(string str)
		{
			return new HtmlStringItem(str);
		}

		public static explicit operator string(HtmlStringItem str)
		{
			return str.ToString();
		}

		public HtmlStringItem()
		{
			StringValue = String.Empty;
		}
		
		public HtmlStringItem(string value)
		{
			StringValue = value;
		}
	}
}