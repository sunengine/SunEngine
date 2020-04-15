using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.Sections
{
	public class ActivitiesClientSection : ClientSection
	{
		[ConfigItem(typeof(CategoriesItem))] public string MaterialsCategories { get; set; } = "Root";
		[ConfigItem(typeof(CategoriesItem))] public string CommentsCategories { get; set; } = "Root";
	}
}