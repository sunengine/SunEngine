namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class TokensItem : ConfigItem
	{
		public TokensItem(string value = "", bool configJs = false) : base(configJs)
		{
			_objectValue = _stringValue = value;
		}
	}
}