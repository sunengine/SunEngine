using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.SectionsData
{
	public class MaterialsClientSectionData : CategoryClientSectionData
	{
		[ConfigItem(typeof(BooleanItem))] public bool ShowAuthor { get; set; } = true;
		[ConfigItem(typeof(BooleanItem))] public bool ShowPublishDate { get; set; } = true;
		[ConfigItem(typeof(BooleanItem))] public bool ShowComments { get; set; } = true;
	}
}