using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.DataBase;
using SunEngine.Models;
using SunEngine.Presenters.PagedList;
using SunEngine.Services;

namespace SunEngine.Presenters
{
    public interface IBlogPresenter
    {
        Task<IPagedList<PostViewModel>> GetPostsAsync(int categoryId, int page, int pageSize);

        Task<IPagedList<PostViewModel>>
            GetCategoriesPostsAsync(int[] categoriesIds, int page, int pageSize);
    }

    public class BlogPresenter : DbService, IBlogPresenter
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
                x => x.OrderByDescending(y => y.PublishDate),
                page,
                pageSize);
        }
        
        public virtual Task<IPagedList<PostViewModel>> GetCategoriesPostsAsync(int[] categoriesIds, int page, int pageSize)
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
                    CategoryTitle = x.Category.Title,
                    HasMoreText = x.Text.Length != x.Preview.Length
                },
                x => categoriesIds.Contains(x.CategoryId),
                x => x.OrderByDescending(y => y.PublishDate),
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
        public string CategoryTitle { get; set; }
        public bool HasMoreText { get; set; }
    }
}