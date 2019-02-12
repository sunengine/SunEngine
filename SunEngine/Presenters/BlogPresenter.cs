using System;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.Controllers;
using SunEngine.DataBase;
using SunEngine.Presenters.PagedList;
using SunEngine.Services;

namespace SunEngine.Presenters
{
    public class BlogPresenter : DbService
    {
        public BlogPresenter(DataBaseConnection db) : base(db)
        {
        }

        public virtual Task<IPagedList<PostViewModel>> GetPostsAsync(int categoryId, int page, int pageSize)
        {
            return db.MaterialsNotDeleted.GetPagedListAsync(
                x => new PostViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Preview = x.Preview,
                    MessagesCount = x.MessagesCount,
                    AuthorName = x.Author.UserName,
                    AuthorLink = x.Author.Link,
                    AuthorAvatar = x.Author.Avatar,
                    PublishDate = x.PublishDate,
                    CategoryName = x.Category.Name.ToLower(),
                    HasMoreText = x.Text.Length != x.Preview.Length
                },
                x => x.CategoryId == categoryId,
                x => x.OrderByDescending(y => y.LastActivity),
                page,
                pageSize);
        }
    }

    public class PostViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string AuthorLink { get; set; }
        public string AuthorAvatar { get; set; }
        public string Preview { get; set; }
        public int MessagesCount { get; set; }
        public DateTime PublishDate { get; set; }
        public string CategoryName { get; set; }
        public bool HasMoreText { get; set; }
    }
}