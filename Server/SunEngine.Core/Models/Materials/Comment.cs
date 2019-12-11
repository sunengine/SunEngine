using System;

namespace SunEngine.Core.Models.Materials
{
    /// <summary>
    /// Comment for Material
    /// </summary>
    public class Comment
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }

        public DateTime PublishDate { get; set; }
        public DateTime? EditDate { get; set; }

        public int MaterialId { get; set; }
        public Material Material { get; set; }

        public DateTime? DeletedDate { get; set; }
    }
}
