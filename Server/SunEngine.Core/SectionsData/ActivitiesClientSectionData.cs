using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.SectionsData
{
	public class ActivitiesClientSectionData : ClientSectionData
	{
		[ConfigItem(typeof(CategoriesItem))] public string MaterialsCategories { get; set; }
		[ConfigItem(typeof(CategoriesItem))] public string MaterialsCategoriesExclude { get; set; }
	}
}