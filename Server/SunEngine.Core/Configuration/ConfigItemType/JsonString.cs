using SunEngine.Core.Utils;

namespace SunEngine.Core.Configuration
{
	public class JsonString : ConfigItem
	{
		public override bool Validate()
		{
			return StringValue.ValidateJson();
		}

		public JsonString(string value = "{}", bool jsConfig = false) : base(jsConfig)
		{
			StringValue = value.Replace("'", "\"");
		}
	}
}