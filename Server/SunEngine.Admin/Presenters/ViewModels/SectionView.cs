using System.Collections.Generic;
using SunEngine.Core.Models;

namespace SunEngine.Admin.Presenters
{
	public class SectionView
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Token { get; set; }
		public string Type { get; set; }
		public string Roles { get; set; }
		public bool IsCacheData { get; set; }
		public ConfigItemView[] Options { get; set; }

		public Dictionary<string, string[]> Enums { get; set; }

		public SectionView(Section section)
		{
			Id = section.Id;
			Name = section.Name;
			Token = section.Token;
			Type = section.Type;
			Roles = section.Roles;
			IsCacheData = section.IsCacheData;
		}
	}
}