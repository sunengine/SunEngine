using LinqToDB;
using LinqToDB.Mapping;

namespace SunEngine.Core.Models
{
	public class Section
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Token { get; set; }
		public string Type { get; set; }
		public string Roles { get; set; }
		public bool IsCacheData { get; set; }
		[Column(DataType = DataType.Json)]
		public string Options { get; set; }
	}
	
	public static class SectionsGroupsNames
	{
		public static string Pages = "Pages";
		public static string CategoriesPresentations = "CategoriesPresentations";
		public static string Previews = "Previews";
	}
}