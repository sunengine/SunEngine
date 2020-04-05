using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.Extensions.ObjectPool;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Services;
using SunEngine.Core.Utils.PagedList;

namespace SunEngine.Core.Presenters
{
	public interface IForumPresenter
	{
		Task<IPagedList<TopicInfoView>> GetThreadAsync(MaterialsShowOptions options);
		Task<IPagedList<TopicInfoView>> GetNewTopicsAsync(MaterialsMultiCatShowOptions options, int maxPages);
	}

	public class ForumPresenter : DbService, IForumPresenter, IMaterialsQueryPresenter
	{
		public ForumPresenter(DataBaseConnection db) : base(db)
		{
		}


		public virtual Task<IPagedList<TopicInfoView>> GetThreadAsync(MaterialsShowOptions options)
		{
			IQueryable<Material> query = db.Materials;

			if (!options.ShowHidden)
				query = query.Where(x => !x.IsHidden);

			if (!options.ShowDeleted)
				query = query.Where(x => x.DeletedDate == null);

			return query.GetPagedListAsync(
				x => new TopicInfoView
				{
					Id = x.Id,
					Title = x.Title,
					SubTitle = x.SubTitle,
					CommentsCount = x.CommentsCount,
					AuthorName = x.Author.UserName,
					AuthorAvatar = x.Author.Avatar,
					PublishDate = x.PublishDate,
					LastCommentId = x.LastCommentId,
					LastCommentPublishDate = x.LastCommentId.HasValue ? (DateTime?) x.LastComment.PublishDate : null,
					CategoryName = x.Category.Name,
					LastCommentAuthorName = x.LastComment.Author.UserName,
					LastCommentAuthorAvatar = x.LastComment.Author.Avatar,
					IsCommentsBlocked = x.IsCommentsBlocked,
					IsHidden = x.IsHidden,
					DeletedDate = x.DeletedDate
				},
				x => x.CategoryId == options.CategoryId,
				x => x.OrderByDescending(y => y.LastActivity),
				options.Page,
				options.PageSize);
		}


		public virtual Task<IPagedList<TopicInfoView>> GetNewTopicsAsync(MaterialsMultiCatShowOptions options,
			int maxPages)
		{
			return db.MaterialsVisible.GetPagedListMaxAsync(
				x => new TopicInfoView
				{
					Id = x.Id,
					Title = x.Title,
					SubTitle = x.SubTitle,
					CommentsCount = x.CommentsCount,
					AuthorName = x.Author.UserName,
					AuthorAvatar = x.Author.Avatar,
					PublishDate = x.PublishDate,
					LastCommentId = x.LastCommentId,
					LastCommentPublishDate = x.LastCommentId.HasValue ? (DateTime?) x.LastComment.PublishDate : null,
					CategoryName = x.Category.Name,
					CategoryTitle = x.Category.Title,
					LastCommentAuthorName = x.LastComment.Author.UserName,
					LastCommentAuthorAvatar = x.LastComment.Author.Avatar,
					IsCommentsBlocked = x.IsCommentsBlocked
				},
				x => options.CategoriesIds.Contains(x.CategoryId),
				x => x.OrderByDescending(y => y.LastActivity),
				options.Page,
				options.PageSize,
				maxPages);
		}

    public async Task<IList<object>> GetMaterialsByCategoryAsync(MaterialsShowOptions options)
    {
      Func<IQueryable<Material>, IOrderedQueryable<Material>> orderBy;

      if (options.Sort != null)
        orderBy = options.Sort;
      else
        orderBy = MaterialsDefaultSortService.DefaultSortOptions.GetValueOrDefault(nameof(ForumPresenter));
      
      var result = await db.MaterialsVisible.GetPagedListMaxAsync(x => new TopicInfoView()
        {
          Id = x.Id,
          Title = x.Title,
          SubTitle = x.SubTitle,
          CommentsCount = x.CommentsCount,
          AuthorName = x.Author.UserName,
          AuthorAvatar = x.Author.Avatar,
          PublishDate = x.PublishDate,
          LastCommentId = x.LastCommentId,
          LastCommentPublishDate = x.LastCommentId.HasValue ? (DateTime?) x.LastComment.PublishDate : null,
          CategoryName = x.Category.Name,
          CategoryTitle = x.Category.Title,
          LastCommentAuthorName = x.LastComment.Author.UserName,
          LastCommentAuthorAvatar = x.LastComment.Author.Avatar,
          IsCommentsBlocked = x.IsCommentsBlocked
        }, x => x.CategoryId == options.CategoryId,
        orderBy,
        options.Page,
        options.PageSize);

      return result.Items as IList<object>;
    }

    public async Task<IList<object>> GetMaterialsFromMultiCategory(MaterialsMultiCatShowOptions options)
    {
      Func<IQueryable<Material>, IOrderedQueryable<Material>> order;
      if (options.SortType != null)
        order = options.SortType;
      else
        order = MaterialsDefaultSortService.DefaultSortOptions.GetValueOrDefault(nameof(ArticlesPresenter));

      var result = await db.MaterialsVisible.GetPagedListAsync(x => new TopicInfoView()
        {
          Id = x.Id,
          Title = x.Title,
          SubTitle = x.SubTitle,
          CommentsCount = x.CommentsCount,
          AuthorName = x.Author.UserName,
          AuthorAvatar = x.Author.Avatar,
          PublishDate = x.PublishDate,
          LastCommentId = x.LastCommentId,
          LastCommentPublishDate = x.LastCommentId.HasValue ? (DateTime?) x.LastComment.PublishDate : null,
          CategoryName = x.Category.Name,
          CategoryTitle = x.Category.Title,
          LastCommentAuthorName = x.LastComment.Author.UserName,
          LastCommentAuthorAvatar = x.LastComment.Author.Avatar,
          IsCommentsBlocked = x.IsCommentsBlocked
        }, x => options.CategoriesIds.Contains(x.CategoryId),
        order,
        options.Page,
        options.PageSize);

      return result.Items as IList<Object>;
    }
  }

	/// <summary>
	/// Topic inside Thread on Client view
	/// </summary>
	public class TopicInfoView
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string SubTitle { get; set; }
		public string AuthorName { get; set; }
		public string AuthorAvatar { get; set; }
		public int CommentsCount { get; set; }
		public DateTime PublishDate { get; set; }
		public int? LastCommentId { get; set; }
		public string LastCommentAuthorName { get; set; }
		public string LastCommentAuthorAvatar { get; set; }
		public DateTime? LastCommentPublishDate { get; set; }
		public string CategoryTitle { get; set; }
		public string CategoryName { get; set; }
		public bool IsCommentsBlocked { get; set; }
		public bool IsHidden { get; set; }
		public DateTime? DeletedDate { get; set; }
	}
}
