using SunEngine.Core.Utils;

namespace SunEngine.Core.Configuration
{
	public class JsonString : ConfigItem
	{
		public static implicit operator JsonString(string str)
		{
			return new JsonString(str.Replace("'", "\""));
		}

		public static explicit operator string(JsonString str)
		{
			return str.ToString();
		}

		public override bool Validate()
		{
			return StringValue.ValidateJson();
		}

		public JsonString(string value)
		{
			StringValue = value.Replace("'", "\"");
		}
	}
}