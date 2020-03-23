using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.SectionsData
{
	public class ClientSectionData
	{
		[ConfigItem(typeof(StringItem))] public string Title { get; set; }
		[ConfigItem(typeof(StringItem))] public string SubTitle { get; set; }
		[ConfigItem(typeof(StringItem))] public string Header { get; set; }
	}
}