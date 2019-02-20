using System;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.DataBase;
using SunEngine.Presenters.PagedList;
using SunEngine.Services;

namespace SunEngine.Presenters
{
    public interface IArticlesPresenter
    {
        Task<IPagedList<ArticleInfoViewModel>> GetArticlesAsync(int categoryId, int page, int pageSize);
    }

    public class ArticlesPresenter : DbService, IArticlesPresenter
    {
        public ArticlesPresenter(DataBaseConnection db) : base(db)
        {
        }

        public virtual Task<IPagedList<ArticleInfoViewModel>> GetArticlesAsync(int categoryId, int page, int pageSize)
        {
            return db.MaterialsNotDeleted.GetPagedListAsync(
                x => new ArticleInfoViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Description = x.Description,
                    MessagesCount = x.MessagesCount,
                    AuthorName = x.Author.UserName,
                    PublishDate = x.PublishDate,
                    CategoryName = x.Category.Name.ToLower()
                },
                x => x.CategoryId == categoryId,
                x => x.OrderByDescending(y => y.PublishDate),
                page,
                pageSize);
        }
    }

    public class ArticleInfoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public int MessagesCount { get; set; }
        public DateTime PublishDate { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryName { get; set; }
    }
}