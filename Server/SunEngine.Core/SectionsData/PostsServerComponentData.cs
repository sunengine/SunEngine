using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.Configuration.Sections
{
	public class PostsServerComponentData
	{
		[ConfigItem(typeof(CategoriesItem))]
		public string Categories { get; set; }
		[ConfigItem(typeof(CategoriesItem))]
		public string CategoriesExclude { get; set; }
		[ConfigItem(typeof(IntegerItem))]
		public int PreviewSize { get; set; }
		[ConfigItem(typeof(IntegerItem))]
		public int PageSize { get; set; }
	}
}