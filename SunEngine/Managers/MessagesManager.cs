using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.TextProcess;
using SunEngine.Services;

namespace SunEngine.Managers
{
    public class MessagesManager : DbService
    {
        private readonly MaterialsManager materialsManager;
        private readonly Sanitizer _sanitizer;

        public MessagesManager(DataBaseConnection db, MaterialsManager materialsManager,
            Sanitizer sanitizer) : base(db)
        {
            this.materialsManager = materialsManager;
            this._sanitizer = sanitizer;
        }

        public async Task InsertAsync(Message message)
        {
            message.Text = _sanitizer.Sanitize(message.Text);
            message.Id = await db.InsertWithInt32IdentityAsync(message);
            await materialsManager.DetectAndSetLastMessageAndCountAsync(message.MaterialId);
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
            await materialsManager.DetectAndSetLastMessageAndCountAsync(message.MaterialId);
        }

        public async Task UpdateAsync(Message message)
        {
            message.Text = _sanitizer.Sanitize(message.Text);
            await db.UpdateAsync(message);
            await materialsManager.DetectAndSetLastMessageAndCountAsync(message.MaterialId);
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
    }
}