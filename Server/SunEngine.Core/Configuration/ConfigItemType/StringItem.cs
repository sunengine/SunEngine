namespace SunEngine.Core.Configuration
{
	public class StringItem : ConfigItem
	{
		public StringItem(string value = "", bool configJs = false) : base(configJs)
		{
			_objectValue = _stringValue = value;
		}
	}
}