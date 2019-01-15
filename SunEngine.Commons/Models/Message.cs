using System;
using System.Collections.Generic;

namespace SunEngine.Commons.Models
{
    public class Message
    {
        public int Id { get; set; }

        public string Text { get; set; }

        public int AuthorId { get; set; }
        public User Author { get; set; }

        public DateTime PublishDate { get; set; }
        public DateTime EditDate { get; set; }

        public int MaterialId { get; set; }
        public Material Material { get; set; }

        //public int? ParentId { get; set; }
        //public Message Parent { get; set; }
        
        public bool IsDeleted { get; set; }

        //public virtual ICollection<Message> Children { get; set; }
    }
}
