using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.SectionsData
{
	public class ActivitiesClientSectionData
	{
		[ConfigItem(typeof(StringItem))] public string Title { get; set; } = "Activities";
		[ConfigItem(typeof(StringItem))] public string SubTitle { get; set; }
		[ConfigItem(typeof(StringItem))] public string Header { get; set; }
		[ConfigItem(typeof(CategoriesItem))] public string MaterialsCategories { get; set; }
		[ConfigItem(typeof(CategoriesItem))] public string MaterialsCategoriesExclude { get; set; }
	}
}