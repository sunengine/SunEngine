using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.Extensions.Options;
using Npgsql;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.TextProcess;
using SunEngine.Options;

namespace SunEngine.EntityServices
{
    public class MaterialsService : DbService
    {
        private readonly TagsService tagsService;
        private readonly Sanitizer _sanitizer;
        private readonly MaterialOptions materialOptions;


        public MaterialsService(DataBaseConnection db,
            Sanitizer sanitizer,
            TagsService tagsService,
            IOptions<MaterialOptions> materialOptions) : base(db)
        {
            this.tagsService = tagsService;
            this._sanitizer = sanitizer;
            this.materialOptions = materialOptions.Value;
        }


        public Task<MaterialViewModel> GetViewModelAsync(int id)
        {
            return db.Materials.Where(x => x.Id == id).Select(x =>
                new MaterialViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    AuthorLink = x.Author.Link,
                    AuthorName = x.Author.UserName,
                    AuthorAvatar = x.Author.Avatar,
                    AuthorId = x.Author.Id,
                    PublishDate = x.PublishDate,
                    EditDate = x.EditDate,
                    MessagesCount = x.MessagesCount,
                    Text = x.Text,
                    CategoryName = x.Category.Name.ToLower(),
                    IsDeleted = x.IsDeleted,
                    Tags = x.TagMaterials.OrderBy(y => y.Tag.Name).Select(y => y.Tag.Name).ToArray()
                }
            ).FirstOrDefaultAsync();
        }

        public async Task<int?> GetCategoryIdIfHasMaterialAsync(int id)
        {
            return await db.Materials.Where(x => x.Id == id).Select(x => x.Category.Id).FirstOrDefaultAsync();
        }

        public Task<Material> GetAsync(int id)
        {
            return db.Materials.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task InsertAsync(Material material, string tags)
        {
            material.Text = _sanitizer.Sanitize(material.Text);

            material.MakePreviewAndDescription(materialOptions.MaterialDescriptionLength,
                materialOptions.MaterialPreviewLength);


            material.Id = await db.InsertWithInt32IdentityAsync(material);

            await tagsService.MaterialCreateAndSetTagsAsync(material, tags);
        }

        public async Task UpdateAsync(Material material, string tags)
        {
            material.Text =
                _sanitizer.Sanitize(material
                    .Text); // TODO сделать совместную валидацию, санитайзин и превью на основе одного DOM

            material.MakePreviewAndDescription(materialOptions.MaterialDescriptionLength,
                materialOptions.MaterialPreviewLength);

            await db.UpdateAsync(material);

            await tagsService.MaterialCreateAndSetTagsAsync(material, tags);
        }

        public async Task MoveToTrashAsync(Material material)
        {
            await db.Materials.Where(x => x.Id == material.Id).Set(x => x.IsDeleted, true).UpdateAsync();
        }

        public async Task DetectAndSetLastMessageAndCountAsync(Material material)
        {
            var messagesQuery = db.Messages.Where(x => x.MaterialId == material.Id);

            var lastMessage = await messagesQuery.OrderByDescending(x => x.PublishDate).FirstOrDefaultAsync();
            var lastMessageId = lastMessage?.Id;
            var lastActivity = lastMessage?.PublishDate;
            if (lastActivity == null)
                lastActivity = material.PublishDate;

            var messagesCount = await messagesQuery.CountAsync();

            await db.Materials.Where(x => x.Id == material.Id)
                .Set(x => x.MessagesCount, x => messagesCount)
                .Set(x => x.LastMessageId, x => lastMessageId)
                .Set(x => x.LastActivity, x => lastActivity)
                .UpdateAsync();
        }

        public Task DetectAndSetLastMessageAndCountAsync(int materialId)
        {
            return DetectAndSetLastMessageAndCountAsync(db.Materials.FirstOrDefault(x => x.Id == materialId));
        }
    }

    public class MaterialViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string AuthorName { get; set; }
        public int AuthorId { get; set; }
        public string AuthorLink { get; set; }
        public string AuthorAvatar { get; set; }
        public int MessagesCount { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime? EditDate { get; set; }
        public string CategoryName { get; set; }
        public bool IsDeleted { get; set; }
        public string[] Tags { get; set; }
    }
}