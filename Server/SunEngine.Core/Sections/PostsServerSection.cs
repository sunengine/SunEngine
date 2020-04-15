using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.Sections
{
	public class PostsServerSection : ServerSection
	{
		[ConfigItem(typeof(CategoriesItem))] public string Categories { get; set; } = "Root";
		[ConfigItem(typeof(CategoriesItem))] public string CategoriesExclude { get; set; }
		[ConfigItem(typeof(IntegerItem))] public int PreviewSize { get; set; } = 800;
		[ConfigItem(typeof(IntegerItem))] public int PageSize { get; set; } = 12;
	}
}