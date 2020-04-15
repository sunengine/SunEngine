using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.SectionsData
{
	public class CategoryClientSectionData : ClientSectionData
	{
		[ConfigItem(typeof(CategoriesItem))] public string CategoriesNames { get; set; }
	}
}