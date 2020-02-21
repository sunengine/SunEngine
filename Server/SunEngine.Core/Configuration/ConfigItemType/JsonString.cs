using SunEngine.Core.Utils;

namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class JsonString : StringItem
	{
		/*public override bool Validate()
		{
			return StringValue.ValidateJson();
		}*/

		public JsonString(string value = "{}", bool configJs = false) 
			: base(value.Replace("'", "\""), configJs)
		{
		}
	}
}