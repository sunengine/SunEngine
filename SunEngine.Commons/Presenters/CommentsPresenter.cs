using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Services;

namespace SunEngine.Commons.Presenters
{
    public interface ICommentsPresenter
    {
        Task<(CommentViewModel commentViewModel, int categoryId)>
            GetCommentAsync(int commentId);

        Task<List<CommentViewModel>> GetMaterialCommentsAsync(int materialId);
    }

    public class CommentsPresenter : DbService, ICommentsPresenter
    {
        public CommentsPresenter(DataBaseConnection db) : base(db)
        {
        }
        
        public virtual async Task<(CommentViewModel commentViewModel, int categoryId)>
            GetCommentAsync(int commentId)
        {
            var rez = await db.Comments.Where(x => x.Id == commentId).Select(x =>
                new
                {
                    commentViewModel = new CommentViewModel
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

            return (rez.commentViewModel, rez.categoryId);
        }

        public virtual Task<List<CommentViewModel>> GetMaterialCommentsAsync(int materialId)
        {
            return db.CommentsNotDeleted.Where(x => x.MaterialId == materialId)
                .OrderBy(x => x.PublishDate)
                .Select(
                    x => new CommentViewModel
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
    
    public class CommentViewModel
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