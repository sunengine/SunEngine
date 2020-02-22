using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.SectionsData
{
	public class ActivitiesServerSectionData
	{
		[ConfigItem(typeof(CategoriesItem))] public string MaterialsCategories { get; set; }
		[ConfigItem(typeof(CategoriesItem))] public string MaterialsCategoriesExclude { get; set; }
		[ConfigItem(typeof(CategoriesItem))] public string CommentsCategories { get; set; }
		[ConfigItem(typeof(CategoriesItem))] public string CommentsCategoriesExclude { get; set; }
		[ConfigItem(typeof(IntegerItem))] public int Number { get; set; }
	}
}