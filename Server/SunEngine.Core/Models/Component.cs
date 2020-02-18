using System.Collections.Generic;

namespace SunEngine.Core.Models
{
	public class Component
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string ComponentTypeName { get; set; }

		public ComponentType ComponentType { get; set; }

		public string Roles { get; set; }

		public bool IsCacheData { get; set; }

		public string JsonOptions { get; set; }

		//public Dictionary<string, object> Options { get; set; }
	}
}