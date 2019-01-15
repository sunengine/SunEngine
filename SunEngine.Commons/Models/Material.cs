using System;
using System.Collections.Generic;
using LinqToDB.Mapping;

namespace SunEngine.Commons.Models
{
    public class Material
    {
        public int Id { get; set; }

        public string Title { get; set; }
        /// <summary>
        /// Для статей используемых в системе для удобства ссылок
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// описание для вывода в виджетах с компактной информацией - Line
        /// </summary>
        public string Description { get; set; }
        
        // Заполнять в ручную не нужно, заполняется в репозитории в методах Insert, Add
        public string Preview { get; set; }
        public string Text { get; set; }

        public int? AuthorId { get; set; }
        public User Author { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Association(ThisKey = "Id", OtherKey = "MaterialId")]
        public virtual ICollection<Message> Messages { get; set; }

        public int? LastMessageId { get; set; }
        public Message LastMessage { get; set; }

        /// <summary>
        /// наибольшая из дат последнего сообщения и самого материала
        /// </summary>
        public DateTime LastActivity { get; set; }

        public DateTime PublishDate { get; set; }
        public DateTime? EditDate { get; set; }

        public int SortNumber { get; set; }
        
        public bool IsDeleted { get; set; }
        
        /// <summary>
        /// Количество не удалённых сообщений
        /// </summary>
        public int MessagesCount { get; set; }

        [Association(ThisKey = "Id", OtherKey = "MaterialId")]
        public virtual ICollection<TagMaterial> TagMaterials { get; set; }

        
    }
}
