using System;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Services;
using SunEngine.Core.Utils.PagedList;

namespace SunEngine.Core.Presenters
{
    public interface IBlogPresenter
    {
        Task<IPagedList<PostView>> GetPostsAsync(MaterialsShowOptions options);

        Task<IPagedList<PostView>>
            GetPostsFromMultiCategoriesAsync(MaterialsMultiCatShowOptions options);
    }

    public class BlogPresenter : DbService, IBlogPresenter
    {
        public BlogPresenter(DataBaseConnection db) : base(db)
        {
        }

        public virtual Task<IPagedList<PostView>> GetPostsAsync(MaterialsShowOptions options)
        {
            IQueryable<Material> query = db.Materials;

            if (!options.ShowHidden)
                query = query.Where(x => !x.IsHidden);

            if (!options.ShowDeleted)
                query = query.Where(x => x.DeletedDate == null);

            return query.GetPagedListAsync(
                x => new PostView
                {
                    Id = x.Id,
                    Title = x.Title,
                    Preview = x.Preview,
                    CommentsCount = x.CommentsCount,
                    AuthorName = x.Author.UserName,
                    AuthorLink = x.Author.Link,
                    AuthorAvatar = x.Author.Avatar,
                    PublishDate = x.PublishDate,
                    CategoryName = x.Category.Name,
                    HasMoreText = x.Text.Length != x.Preview.Length,
                    IsCommentsBlocked = x.IsCommentsBlocked,
                    IsHidden = x.IsHidden,
                    DeletedDate = x.DeletedDate
                },
                x => x.CategoryId == options.CategoryId,
                x => x.OrderByDescending(y => y.PublishDate),
                options.Page,
                options.PageSize);
        }

        public virtual Task<IPagedList<PostView>> GetPostsFromMultiCategoriesAsync(MaterialsMultiCatShowOptions options)
        {
            return db.MaterialsVisible.GetPagedListAsync(
                x => new PostView
                {
                    Id = x.Id,
                    Title = x.Title,
                    Preview = x.Preview,
                    CommentsCount = x.CommentsCount,
                    AuthorName = x.Author.UserName,
                    AuthorLink = x.Author.Link,
                    AuthorAvatar = x.Author.Avatar,
                    PublishDate = x.PublishDate,
                    CategoryName = x.Category.Name,
                    CategoryTitle = x.Category.Title,
                    HasMoreText = x.Text.Length != x.Preview.Length,
                    IsCommentsBlocked = x.IsCommentsBlocked
                },
                x => options.CategoriesIds.Contains(x.CategoryId),
                x => x.OrderByDescending(y => y.PublishDate),
                options.Page,
                options.PageSize);
        }
    }

    public class PostView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string AuthorLink { get; set; }
        public string AuthorAvatar { get; set; }
        public string Preview { get; set; }
        public int CommentsCount { get; set; }
        public DateTime PublishDate { get; set; }
        public string CategoryName { get; set; }
        public string CategoryTitle { get; set; }
        public bool HasMoreText { get; set; }
        public bool IsCommentsBlocked { get; set; }
        public bool IsHidden { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
