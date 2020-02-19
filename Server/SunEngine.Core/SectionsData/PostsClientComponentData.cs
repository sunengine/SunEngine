using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.Configuration.Sections
{
	public class PostsClientComponentData
	{
		[ConfigItem(typeof(BooleanItem))]
		public bool ShowCaption { get; set; }
		[ConfigItem(typeof(BooleanItem))]
		public bool ShowAuthor { get; set; }
		[ConfigItem(typeof(BooleanItem))]
		public bool ShowComments { get; set; }
		[ConfigItem(typeof(BooleanItem))]
		public bool ShowPublishDate { get; set; }
	}
}