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
        Task<IPagedList<ArticleInfoView>> GetArticlesAsync(MaterialsShowOptions options);

        Task<IPagedList<ArticleInfoView>> GetArticlesFromMultiCategoriesAsync(MaterialsMultiCatShowOptions options);
    }


    public enum OrderType
    {
        PublishDate = 0,
        SortNumber = 1
    }

    public class ArticlesPresenter : DbService, IArticlesPresenter
    {
        public ArticlesPresenter(DataBaseConnection db) : base(db)
        {
        }

        public virtual Task<IPagedList<ArticleInfoView>> GetArticlesAsync(MaterialsShowOptions options)
        {
            Func<IQueryable<Material>, IOrderedQueryable<Material>> orderBy;
            if (options.orderType == OrderType.PublishDate)
                orderBy = x => x.OrderByDescending(y => y.PublishDate);
            else
                orderBy = x => x.OrderByDescending(y => y.SortNumber);

            IQueryable<Material> query = db.Materials;

            if (!options.ShowHidden)
                query = query.Where(x => !x.IsHidden);

            if (!options.ShowDeleted)
                query = query.Where(x => x.DeletedDate == null);


            return query.GetPagedListAsync(
                x => new ArticleInfoView
                {
                    Id = x.Id,
                    Name = x.Name,
                    Title = x.Title,
                    Description = x.SubTitle,
                    CommentsCount = x.CommentsCount,
                    AuthorName = x.Author.UserName,
                    PublishDate = x.PublishDate,
                    CategoryName = x.Category.Name,
                    SortNumber = x.SortNumber,
                    IsHidden = x.IsHidden,
                    DeletedDate = x.DeletedDate,
                    IsCommentsBlocked = x.IsCommentsBlocked
                },
                x => x.CategoryId == options.CategoryId,
                orderBy,
                options.Page,
                options.PageSize);
        }

        public virtual Task<IPagedList<ArticleInfoView>> GetArticlesFromMultiCategoriesAsync(
            MaterialsMultiCatShowOptions options)
        {
            return db.Materials.Where(x => x.DeletedDate == null && !x.IsHidden).GetPagedListAsync(
                x => new ArticleInfoView
                {
                    Id = x.Id,
                    Name = x.Name,
                    Title = x.Title,
                    Description = x.SubTitle,
                    CommentsCount = x.CommentsCount,
                    AuthorName = x.Author.UserName,
                    PublishDate = x.PublishDate,
                    CategoryName = x.Category.Name,
                    CategoryTitle = x.Category.Title,
                    IsCommentsBlocked = x.IsCommentsBlocked
                },
                x => options.CategoriesIds.Contains(x.CategoryId),
                x => x.OrderByDescending(y => y.PublishDate),
                options.Page,
                options.PageSize);
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
        public bool IsCommentsBlocked { get; set; }
        public bool IsHidden { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
