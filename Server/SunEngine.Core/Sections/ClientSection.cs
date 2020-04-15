using SunEngine.Core.Configuration.ConfigItemType;

namespace SunEngine.Core.Sections
{
	public class ClientSection
	{
		[ConfigItem(typeof(StringItem))] public string Title { get; set; }
		[ConfigItem(typeof(StringItem))] public string SubTitle { get; set; }
		[ConfigItem(typeof(StringItem))] public string Header { get; set; }
	}
}