using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.Extensions.Options;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.TextProcess;
using SunEngine.Configuration.Options;
using SunEngine.Services;

namespace SunEngine.Managers
{
    public class MaterialsManager : DbService
    {
        private readonly TagsManager tagsManager;
        private readonly Sanitizer _sanitizer;
        private readonly MaterialOptions materialOptions;


        public MaterialsManager(DataBaseConnection db,
            Sanitizer sanitizer,
            TagsManager tagsManager,
            IOptions<MaterialOptions> materialOptions) : base(db)
        {
            this.tagsManager = tagsManager;
            this._sanitizer = sanitizer;
            this.materialOptions = materialOptions.Value;
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

            await tagsManager.MaterialCreateAndSetTagsAsync(material, tags);
        }

        public async Task UpdateAsync(Material material, string tags)
        {
            material.Text =
                _sanitizer.Sanitize(material
                    .Text); // TODO сделать совместную валидацию, санитайзин и превью на основе одного DOM

            material.MakePreviewAndDescription(materialOptions.MaterialDescriptionLength,
                materialOptions.MaterialPreviewLength);

            await db.UpdateAsync(material);

            await tagsManager.MaterialCreateAndSetTagsAsync(material, tags);
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
}