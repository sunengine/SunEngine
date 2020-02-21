namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class CategoriesItem : StringItem
	{
		public CategoriesItem(string value = "") : base(value)
		{
		}
		
		public CategoriesItem(string value, bool configJs) : base(value ?? "", configJs)
		{
		}
	}
}