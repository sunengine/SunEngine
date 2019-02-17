using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.DataBase;
using SunEngine.Models.Materials;
using SunEngine.Services;
using SunEngine.Utils.TextProcess;

namespace SunEngine.Managers
{
    public interface IMessagesManager
    {
        Task InsertAsync(Message message);
        Task<(Message message, int categoryId)> GetAsync(int messageId);
        Task DeleteAsync(Message message);
        Task UpdateAsync(Message message);
        Task MoveToTrashAsync(Message message);
        Task RestoreFromTrash(Message message);
    }

    public class MessagesManager : DbService, IMessagesManager
    {
        protected readonly IMaterialsManager materialsManager;
        protected readonly Sanitizer sanitizer;

        public MessagesManager(
            DataBaseConnection db, 
            IMaterialsManager materialsManager,
            Sanitizer sanitizer) : base(db)
        {
            this.materialsManager = materialsManager;
            this.sanitizer = sanitizer;
        }

        public virtual async Task InsertAsync(Message message)
        {
            message.Text = sanitizer.Sanitize(message.Text);
            message.Id = await db.InsertWithInt32IdentityAsync(message);
            await materialsManager.DetectAndSetLastMessageAndCountAsync(message.MaterialId);
        }


        public virtual async Task<(Message message, int categoryId)> GetAsync(int messageId)
        {
            var rez = await db.Messages.Where(x => x.Id == messageId).Select(x =>
                new {message = x, categoryId = x.Material.CategoryId}
            ).FirstOrDefaultAsync();
            return (rez.message, rez.categoryId);
        }

        public virtual async Task DeleteAsync(Message message)
        {
            await db.DeleteAsync(message);
            await materialsManager.DetectAndSetLastMessageAndCountAsync(message.MaterialId);
        }

        public virtual async Task UpdateAsync(Message message)
        {
            message.Text = sanitizer.Sanitize(message.Text);
            await db.UpdateAsync(message);
            await materialsManager.DetectAndSetLastMessageAndCountAsync(message.MaterialId);
        }

        public virtual Task MoveToTrashAsync(Message message)
        {
            message.IsDeleted = true;
            return UpdateAsync(message);
        }

        public virtual Task RestoreFromTrash(Message message)
        {
            message.IsDeleted = true;
            return UpdateAsync(message);
        }
    }
}