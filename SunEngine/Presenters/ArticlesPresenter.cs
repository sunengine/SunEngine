using System;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.Controllers;
using SunEngine.DataBase;
using SunEngine.Presenters.PagedList;
using SunEngine.Services;

namespace SunEngine.Presenters
{
    public class ArticlesPresenter : DbService
    {
        public ArticlesPresenter(DataBaseConnection db) : base(db)
        {
        }

        public Task<IPagedList<ArticleInfoViewModel>> GetArticlesAsync(int categoryId, int page, int pageSize)
        {
            return db.MaterialsNotDeleted.GetPagedListAsync(
                x => new ArticleInfoViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    MessagesCount = x.MessagesCount,
                    AuthorName = x.Author.UserName,
                    PublishDate = x.PublishDate,
                    CategoryName = x.Category.Name.ToLower()
                },
                x => x.CategoryId == categoryId,
                x => x.OrderByDescending(y => y.LastActivity),
                page,
                pageSize);
        }
    }

    public class ArticleInfoViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public int MessagesCount { get; set; }
        public DateTime PublishDate { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryName { get; set; }
    }
}