namespace SunEngine.Core.Configuration.ConfigItemType
{
	public class CategoryItem : StringItem
	{
		public CategoryItem(string value = "") : base(value)
		{
		}
		
		public CategoryItem(string value, bool configJs) : base(value ?? "", configJs)
		{
		}
	}
}