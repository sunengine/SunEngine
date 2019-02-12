using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.DataBase;
using SunEngine.Services;

namespace SunEngine.Presenters
{
    public class MessagesPresenter : DbService
    {
        public MessagesPresenter(DataBaseConnection db) : base(db)
        {
        }
        
        public virtual async Task<(MessageViewModel messageViewModel, int categoryId)>
            GetMessageAsync(int messageId)
        {
            var rez = await db.Messages.Where(x => x.Id == messageId).Select(x =>
                new
                {
                    messageViewModel = new MessageViewModel
                    {
                        Id = x.Id,
                        AuthorId = x.AuthorId,
                        AuthorName = x.Author.UserName,
                        AuthorAvatar = x.Author.Avatar,
                        AuthorLink = x.Author.Link,
                        Text = x.Text,
                        PublishDate = x.PublishDate,
                        EditDate = x.EditDate,
                        IsDeleted = x.IsDeleted
                    },
                    categoryId = x.Material.CategoryId
                }
            ).FirstOrDefaultAsync();

            return (rez.messageViewModel, rez.categoryId);
        }

        public virtual Task<List<MessageViewModel>> GetMaterialMessagesAsync(int materialId)
        {
            return db.MessagesNotDeleted.Where(x => x.MaterialId == materialId)
                .OrderBy(x => x.PublishDate)
                .Select(
                    x => new MessageViewModel
                    {
                        Id = x.Id,
                        PublishDate = x.PublishDate,
                        EditDate = x.EditDate,
                        Text = x.Text,
                        AuthorId = x.AuthorId,
                        AuthorName = x.Author.UserName,
                        AuthorLink = x.Author.Link,
                        AuthorAvatar = x.Author.Avatar,
                        IsDeleted = x.IsDeleted
                    }).ToListAsync();
        }

        
    }
    
    public class MessageViewModel
    {
        public int Id { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorLink { get; set; }
        public string AuthorAvatar { get; set; }
        public string Text { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? EditDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}