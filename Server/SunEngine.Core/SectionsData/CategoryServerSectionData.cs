using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.SectionsData
{
	public class CategoryServerSectionData : ServerSectionData
	{
		[ConfigItem(typeof(CategoriesItem))] public string CategoriesNames { get; set; }
		[ConfigItem(typeof(IntegerItem))] public int MaterialsNumber { get; set; }
		[ConfigItem(typeof(EnumItem))] public string MaterialSectionType { get; set; }
	}
}