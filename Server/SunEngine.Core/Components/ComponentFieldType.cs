namespace SunEngine.Core.Models
{
	public enum ComponentFieldTypeType
	{
		String,
		Integer,
		Boolean,
		LongString,
		JsonString,
		HtmlString,
		Roles,
		Category,
		Categories,
		TokensList
	}

	public enum VisType
	{
		Server,
		Client,
		Shared
	}

	public class ComponentFieldType
	{
		public string Name { get; set; }
		public ComponentFieldTypeType Type { get; set; }
		public VisType VisType { get; set; }
	}
}