using SunEngine.Core.Utils;

namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class JsonString : StringItem
	{
		/*public override bool Validate()
		{
			return StringValue.ValidateJson();
		}*/
		public JsonString(string value = "{}")
			: base(value.Replace("'", "\""))
		{
		}

		public JsonString(string value, bool configJs)
			: base(value?.Replace("'", "\"") ?? "{}", configJs)
		{
		}
	}
}