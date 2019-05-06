using System;
using System.Linq;
using System.Threading.Tasks;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Services;
using SunEngine.Core.Utils.PagedList;

namespace SunEngine.Core.Presenters
{
    public interface IArticlesPresenter
    {
        Task<IPagedList<ArticleInfoView>> GetArticlesAsync(int categoryId, ArticlesOrderType order, int page,
            int pageSize);

        Task<IPagedList<ArticleInfoView>> GetArticlesFromMultiCategoriesAsync(int[] categoriesIds, int page,
            int pageSize);
    }

    public enum ArticlesOrderType
    {
        PublishDate = 0,
        SortNumber = 1
    }

    public class ArticlesPresenter : DbService, IArticlesPresenter
    {
        public ArticlesPresenter(DataBaseConnection db) : base(db)
        {
        }

        public virtual Task<IPagedList<ArticleInfoView>> GetArticlesAsync(int categoryId, ArticlesOrderType order,
            int page,
            int pageSize)
        {
            Func<IQueryable<Material>, IOrderedQueryable<Material>> orderBy;
            if (order == ArticlesOrderType.PublishDate)
                orderBy = x => x.OrderByDescending(y => y.PublishDate);
            else
                orderBy = x => x.OrderByDescending(y => y.SortNumber);


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
                    CategoryName = x.Category.Name
                },
                x => x.CategoryId == categoryId,
                orderBy,
                page,
                pageSize);
        }

        public virtual Task<IPagedList<ArticleInfoView>> GetArticlesFromMultiCategoriesAsync(int[] categoriesIds,
            int page, int pageSize)
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
                    CategoryName = x.Category.Name,
                    CategoryTitle = x.Category.Title,
                    SortNumber = x.SortNumber
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

        public int SortNumber { get; set; }
    }
}