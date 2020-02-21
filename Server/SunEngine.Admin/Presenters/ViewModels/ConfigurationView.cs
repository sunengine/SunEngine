using System.Collections.Generic;

namespace SunEngine.Admin.Presenters.ViewModels
{
	public class ConfigurationView
	{
		public List<ConfigItemView> ConfigItems { get; set; } = new List<ConfigItemView>();
		public Dictionary<string, string[]> Enums { get; set; } = new Dictionary<string, string[]>();
	}
}