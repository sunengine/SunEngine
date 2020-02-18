namespace SunEngine.Core.Configuration
{
	public class HtmlString : ConfigItem
	{
		public static implicit operator HtmlString(string str)
		{
			return new HtmlString(str);
		}

		public static explicit operator string(HtmlString str)
		{
			return str.ToString();
		}

		public HtmlString(string value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return Value;
		}
	}
}