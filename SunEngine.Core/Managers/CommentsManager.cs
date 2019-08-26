using System;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Services;
using SunEngine.Core.Utils.TextProcess;

namespace SunEngine.Core.Managers
{
    public interface ICommentsManager
    {
        Task CreateAsync(Comment comment);
        Task<(Comment comment, int categoryId)> GetAsync(int commentId);
        Task DeleteAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task MoveToTrashAsync(Comment comment);
        Task RestoreFromTrash(Comment comment);
    }

    public class CommentsManager : DbService, ICommentsManager
    {
        protected readonly IMaterialsManager materialsManager;
        protected readonly SanitizerService sanitizerService;

        public CommentsManager(
            DataBaseConnection db, 
            IMaterialsManager materialsManager,
            SanitizerService sanitizerService) : base(db)
        {
            this.materialsManager = materialsManager;
            this.sanitizerService = sanitizerService;
        }

        public virtual async Task CreateAsync(Comment comment)
        {
            comment.Text = sanitizerService.Sanitize(comment.Text);
            comment.Id = await db.InsertWithInt32IdentityAsync(comment);
            await materialsManager.DetectAndSetLastCommentAndCountAsync(comment.MaterialId);
        }


        public virtual async Task<(Comment comment, int categoryId)> GetAsync(int commentId)
        {
            var rez = await db.Comments.Where(x => x.Id == commentId).Select(x =>
                new {comment = x, categoryId = x.Material.CategoryId}
            ).FirstOrDefaultAsync();
            return (rez.comment, rez.categoryId);
        }

        public virtual async Task DeleteAsync(Comment comment)
        {
            await db.DeleteAsync(comment);
            await materialsManager.DetectAndSetLastCommentAndCountAsync(comment.MaterialId);
        }

        public virtual async Task UpdateAsync(Comment comment)
        {
            comment.Text = sanitizerService.Sanitize(comment.Text);
            await db.UpdateAsync(comment);
            await materialsManager.DetectAndSetLastCommentAndCountAsync(comment.MaterialId);
        }

        public virtual Task MoveToTrashAsync(Comment comment)
        {
            comment.DeletedDate = DateTime.UtcNow;
            return UpdateAsync(comment);
        }

        public virtual Task RestoreFromTrash(Comment comment)
        {
            comment.DeletedDate = null;
            return UpdateAsync(comment);
        }
    }
}
