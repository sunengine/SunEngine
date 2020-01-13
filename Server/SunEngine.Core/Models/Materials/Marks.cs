namespace SunEngine.Core.Models.Materials
{
	public class MarkType
	{
		public int Id { get; set; }
		public string Name { get; set; }
	}

	public class MaterialMark
	{
		public int MaterialId { get; set; }
		public int Material { get; set; }
		public int MarkTypeId { get; set; }
		public int MarkType { get; set; }
		public string Text { get; set; }
	}

	public class CommentMark
	{
		public int CommentlId { get; set; }
		public int Comment { get; set; }
		public int MarkTypeId { get; set; }
		public int MarkType { get; set; }
		public string Text { get; set; }
	}
}