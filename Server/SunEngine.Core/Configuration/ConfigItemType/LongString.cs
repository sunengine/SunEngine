namespace SunEngine.Core.Configuration
{
	public class LongString : ConfigItem
	{
		public static implicit operator LongString(string str)
		{
			return new LongString(str);
		}

		public static explicit operator string(LongString str)
		{
			return str.ToString();
		}

		public LongString(string value)
		{
			StringValue = value;
		}
	}
}