namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class CategoriesItem : ConfigItem
	{
		public CategoriesItem(string value = "", bool configJs = false) : base(configJs)
		{
			_objectValue = _stringValue = value;
		}
	}
}