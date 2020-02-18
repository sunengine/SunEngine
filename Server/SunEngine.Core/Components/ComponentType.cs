using System.Collections.Generic;

namespace SunEngine.Core.Models
{
	public class ComponentType
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public Dictionary<string, ComponentFieldType> Fields { get; set; }
	}
}