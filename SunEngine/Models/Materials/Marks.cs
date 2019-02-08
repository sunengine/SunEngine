namespace SunEngine.Models.Materials
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

    public class MessageMark
    {
        public int MessagelId { get; set; }
        public int Message { get; set; }
        public int MarkTypeId { get; set; }
        public int MarkType { get; set; }
        public string Text { get; set; }
    }
}