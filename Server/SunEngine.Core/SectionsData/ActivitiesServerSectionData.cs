using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.SectionsData
{
	public class ActivitiesServerSectionData : ServerSectionData
	{
		[ConfigItem(typeof(CategoriesItem))] public string MaterialsCategories { get; set; } = "Root";
		[ConfigItem(typeof(CategoriesItem))] public string MaterialsCategoriesExclude { get; set; }
		[ConfigItem(typeof(CategoriesItem))] public string CommentsCategories { get; set; } = "Root";
		[ConfigItem(typeof(CategoriesItem))] public string CommentsCategoriesExclude { get; set; }
		[ConfigItem(typeof(IntegerItem))] public int Number { get; set; } = 40;
	}
}