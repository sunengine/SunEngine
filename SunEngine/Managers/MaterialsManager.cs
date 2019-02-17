using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.Extensions.Options;
using SunEngine.Configuration.Options;
using SunEngine.DataBase;
using SunEngine.Models.Materials;
using SunEngine.Services;
using SunEngine.Utils.TextProcess;

namespace SunEngine.Managers
{
    public interface IMaterialsManager
    {
        Task<int?> GetMaterialCategoryIdAsync(int materialId);
        Task<Material> GetAsync(int id);
        Task InsertAsync(Material material, string tags);
        Task UpdateAsync(Material material, string tags);
        Task MoveToTrashAsync(Material material);
        Task DetectAndSetLastMessageAndCountAsync(Material material);
        Task DetectAndSetLastMessageAndCountAsync(int materialId);
    }

    public class MaterialsManager : DbService, IMaterialsManager
    {
        protected readonly ITagsManager tagsManager;
        protected readonly Sanitizer sanitizer;
        protected readonly MaterialsOptions materialsOptions;


        public MaterialsManager(DataBaseConnection db,
            Sanitizer sanitizer,
            ITagsManager tagsManager,
            IOptions<MaterialsOptions> materialsOptions) : base(db)
        {
            this.tagsManager = tagsManager;
            this.sanitizer = sanitizer;
            this.materialsOptions = materialsOptions.Value;
        }


        public virtual async Task<int?> GetMaterialCategoryIdAsync(int materialId)
        {
            return await db.Materials.Where(x => x.Id == materialId).Select(x => x.Category.Id).FirstOrDefaultAsync();
        }

        public virtual Task<Material> GetAsync(int id)
        {
            return db.Materials.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task InsertAsync(Material material, string tags)
        {
            material.Text = sanitizer.Sanitize(material.Text);

            material.MakePreviewAndDescription(materialsOptions.DescriptionLength,
                materialsOptions.PreviewLength);

            material.Id = await db.InsertWithInt32IdentityAsync(material);

            await tagsManager.MaterialCreateAndSetTagsAsync(material, tags);
        }

        public virtual async Task UpdateAsync(Material material, string tags)
        {
            material.Text =
                sanitizer.Sanitize(material
                    .Text); // TODO сделать совместную валидацию, санитайзин и превью на основе одного DOM

            material.MakePreviewAndDescription(materialsOptions.DescriptionLength,
                materialsOptions.PreviewLength);

            await db.UpdateAsync(material);

            await tagsManager.MaterialCreateAndSetTagsAsync(material, tags);
        }

        public virtual async Task MoveToTrashAsync(Material material)
        {
            await db.Materials.Where(x => x.Id == material.Id).Set(x => x.IsDeleted, true).UpdateAsync();
        }

        public virtual async Task DetectAndSetLastMessageAndCountAsync(Material material)
        {
            var messagesQuery = db.Messages.Where(x => x.MaterialId == material.Id);

            var lastMessage = await messagesQuery.OrderByDescending(x => x.PublishDate).FirstOrDefaultAsync();
            var lastMessageId = lastMessage?.Id;
            var lastActivity = lastMessage?.PublishDate ?? material.PublishDate;

            var messagesCount = await messagesQuery.CountAsync();

            await db.Materials.Where(x => x.Id == material.Id)
                .Set(x => x.MessagesCount, x => messagesCount)
                .Set(x => x.LastMessageId, x => lastMessageId)
                .Set(x => x.LastActivity, x => lastActivity)
                .UpdateAsync();
        }

        public virtual Task DetectAndSetLastMessageAndCountAsync(int materialId)
        {
            return DetectAndSetLastMessageAndCountAsync(db.Materials.FirstOrDefault(x => x.Id == materialId));
        }
    }
}