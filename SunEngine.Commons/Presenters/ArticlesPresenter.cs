using System;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Services;
using SunEngine.Commons.Utils.PagedList;

namespace SunEngine.Commons.Presenters
{
    public interface IArticlesPresenter
    {
        Task<IPagedList<ArticleInfoView>> GetArticlesAsync(int categoryId, int page, int pageSize);

        Task<IPagedList<ArticleInfoView>> GetArticlesFromMultiCategoriesAsync(int[] categoriesIds, int page,
            int pageSize);

    }

    public class ArticlesPresenter : DbService, IArticlesPresenter
    {
        public ArticlesPresenter(DataBaseConnection db) : base(db)
        {
        }

        public virtual Task<IPagedList<ArticleInfoView>> GetArticlesAsync(int categoryId, int page, int pageSize)
        {
            return db.MaterialsNotDeleted.GetPagedListAsync(
                x => new ArticleInfoView
                {
                    Id = x.Id,
                    Name = x.Name,
                    Title = x.Title,
                    Description = x.Description,
                    CommentsCount = x.CommentsCount,
                    AuthorName = x.Author.UserName,
                    PublishDate = x.PublishDate,
                    CategoryName = x.Category.NameNormalized
                },
                x => x.CategoryId == categoryId,
                x => x.OrderByDescending(y => y.PublishDate),
                page,
                pageSize);
        }
        
        public virtual Task<IPagedList<ArticleInfoView>> GetArticlesFromMultiCategoriesAsync(int[] categoriesIds, int page, int pageSize)
        {
            return db.MaterialsNotDeleted.GetPagedListAsync(
                x => new ArticleInfoView
                {
                    Id = x.Id,
                    Name = x.Name,
                    Title = x.Title,
                    Description = x.Description,
                    CommentsCount = x.CommentsCount,
                    AuthorName = x.Author.UserName,
                    PublishDate = x.PublishDate,
                    CategoryName = x.Category.NameNormalized,
                    CategoryTitle = x.Category.Title
                },
                x => categoriesIds.Contains(x.CategoryId),
                x => x.OrderByDescending(y => y.PublishDate),
                page,
                pageSize);
        }
    }

    public class ArticleInfoView
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AuthorName { get; set; }
        public int CommentsCount { get; set; }
        public DateTime PublishDate { get; set; }
        public string CategoryTitle { get; set; }
        public string CategoryName { get; set; }
    }
}