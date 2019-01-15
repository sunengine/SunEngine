using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.TextProcess;

namespace SunEngine.EntityServices
{
    public class MessagesService : DbService
    {
        private readonly MaterialsService materialsService;
        private readonly Sanitizer _sanitizer;

        public MessagesService(DataBaseConnection db, MaterialsService materialsService,
            Sanitizer sanitizer) : base(db)
        {
            this.materialsService = materialsService;
            this._sanitizer = sanitizer;
        }

        public async Task InsertAsync(Message message)
        {
            message.Text = _sanitizer.Sanitize(message.Text);
            message.Id = await db.InsertWithInt32IdentityAsync(message);
            await materialsService.DetectAndSetLastMessageAndCountAsync(message.MaterialId);
        }

        public async Task<(MessageViewModel messageViewModel, int categoryId)>
            GetViewModelAsync(int messageId)
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

        public async Task<(Message message, int categoryId)> GetAsync(int messageId)
        {
            var rez = await db.Messages.Where(x => x.Id == messageId).Select(x =>
                new {message = x, categoryId = x.Material.CategoryId}
            ).FirstOrDefaultAsync();
            return (rez.message, rez.categoryId);
        }

        public async Task DeleteAsync(Message message)
        {
            await db.DeleteAsync(message);
            await materialsService.DetectAndSetLastMessageAndCountAsync(message.MaterialId);
        }

        public async Task UpdateAsync(Message message)
        {
            message.Text = _sanitizer.Sanitize(message.Text);
            await db.UpdateAsync(message);
            await materialsService.DetectAndSetLastMessageAndCountAsync(message.MaterialId);
        }

        public Task MoveToTrashAsync(Message message)
        {
            message.IsDeleted = true;
            return UpdateAsync(message);
        }

        public Task RestoreFromTrash(Message message)
        {
            message.IsDeleted = true;
            return UpdateAsync(message);
        }

        public Task<List<MessageViewModel>> GetMaterialMessagesAsync(int materialId)
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
}