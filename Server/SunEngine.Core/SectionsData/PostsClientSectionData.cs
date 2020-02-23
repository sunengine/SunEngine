using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.SectionsData
{
	public class PostsClientSectionData
	{
		[ConfigItem(typeof(StringItem))] public string Title { get; set; } = "Posts";
		[ConfigItem(typeof(StringItem))] public string SubTitle { get; set; }
		[ConfigItem(typeof(StringItem))] public string Header { get; set; }
		[ConfigItem(typeof(CategoriesItem))] public string Categories { get; set; } = "Root";
		[ConfigItem(typeof(CategoriesItem))] public string CategoriesExclude { get; set; }
		[ConfigItem(typeof(StringItem))] public string AddButtonLabel { get; set; } = "Add";
		[ConfigItem(typeof(RolesItem))] public string RolesCanAdd { get; set; }
		[ConfigItem(typeof(BooleanItem))] public bool ShowAuthor { get; set; } = true;
		[ConfigItem(typeof(BooleanItem))] public bool ShowPublishDate { get; set; } = true;
		[ConfigItem(typeof(BooleanItem))] public bool ShowReadNext { get; set; } = true;
		[ConfigItem(typeof(BooleanItem))] public bool ShowComments { get; set; } = true;
	}
}