using System.Linq;
using System.Threading.Tasks;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.PagedList;
using SunEngine.Controllers;

namespace SunEngine.EntityServices
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
}