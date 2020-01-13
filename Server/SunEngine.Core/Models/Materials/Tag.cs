using System.Collections.Generic;

namespace SunEngine.Core.Models.Materials
{
	/// <summary>
	/// Tag, Label for Material
	/// </summary>
	public class Tag
	{
		public int Id { get; set; }
		public string Name { get; set; }

		//public int? GroupId { get; set; }
		//public TagSynonymGroup Group { get; set; }

		//public ICollection<TagMaterial> TagMaterials { get; set; }
	}

	public class TagSynonymGroup
	{
		public int Id { get; set; }
		public ICollection<Tag> Tags { get; set; }
	}

	/// <summary>
	/// Many to many relationship between Materials and Tags
	/// </summary>
	public class TagMaterial
	{
		public int TagId { get; set; }
		public Tag Tag { get; set; }

		public int MaterialId { get; set; }
		public Material Material { get; set; }
	}
}