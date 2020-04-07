using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.SectionsData
{
	public class ActivitiesClientSectionData : ClientSectionData
	{
		[ConfigItem(typeof(CategoriesItem))] public string MaterialsCategories { get; set; } = "Root";
		[ConfigItem(typeof(CategoriesItem))] public string CommentsCategories { get; set; } = "Root";
	}
}