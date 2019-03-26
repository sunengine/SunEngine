using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models.Materials;
using SunEngine.Commons.Services;
using SunEngine.Commons.Utils.TextProcess;

namespace SunEngine.Commons.Managers
{
    public interface ICommentsManager
    {
        Task InsertAsync(Comment comment);
        Task<(Comment comment, int categoryId)> GetAsync(int commentId);
        Task DeleteAsync(Comment comment);
        Task UpdateAsync(Comment comment);
        Task MoveToTrashAsync(Comment comment);
        Task RestoreFromTrash(Comment comment);
    }

    public class CommentsManager : DbService, ICommentsManager
    {
        protected readonly IMaterialsManager materialsManager;
        protected readonly Sanitizer sanitizer;

        public CommentsManager(
            DataBaseConnection db, 
            IMaterialsManager materialsManager,
            Sanitizer sanitizer) : base(db)
        {
            this.materialsManager = materialsManager;
            this.sanitizer = sanitizer;
        }

        public virtual async Task InsertAsync(Comment comment)
        {
            comment.Text = sanitizer.Sanitize(comment.Text);
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
            comment.Text = sanitizer.Sanitize(comment.Text);
            await db.UpdateAsync(comment);
            await materialsManager.DetectAndSetLastCommentAndCountAsync(comment.MaterialId);
        }

        public virtual Task MoveToTrashAsync(Comment comment)
        {
            comment.IsDeleted = true;
            return UpdateAsync(comment);
        }

        public virtual Task RestoreFromTrash(Comment comment)
        {
            comment.IsDeleted = true;
            return UpdateAsync(comment);
        }
    }
}