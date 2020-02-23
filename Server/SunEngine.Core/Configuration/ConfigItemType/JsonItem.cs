using SunEngine.Core.Utils;

namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class JsonItem : StringItem
	{
		/*public override bool Validate()
		{
			return StringValue.ValidateJson();
		}*/
		public JsonItem(string value = "{}")
			: base(value.Replace("'", "\""))
		{
		}

		public JsonItem(string value, bool configJs)
			: base(value?.Replace("'", "\"") ?? "{}", configJs)
		{
		}
	}
}