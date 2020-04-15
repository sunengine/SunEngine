using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.SectionsData
{
	public class MaterialsServerSectionData : ServerSectionData
	{
		[ConfigItem(typeof(CategoriesItem))] public string CategoriesNames { get; set; }
		[ConfigItem(typeof(IntegerItem))] public int PageSize { get; set; }= 20;
	}
}
