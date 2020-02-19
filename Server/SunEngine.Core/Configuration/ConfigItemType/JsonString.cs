using SunEngine.Core.Utils;

namespace SunEngine.Core.Configuration
{
	public class JsonString : ConfigItem
	{
		public override bool Validate()
		{
			return StringValue.ValidateJson();
		}

		public JsonString(string value = "{}", bool configJs = false) : base(configJs)
		{
			StringValue = value.Replace("'", "\"");
		}
	}
}