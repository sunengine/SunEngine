using System;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Parser.Html;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Services;
using SunEngine.Core.Utils.PagedList;
using SunEngine.Core.Utils.TextProcess;

namespace SunEngine.Core.Presenters
{
	public interface IBlogPresenter
	{
		Task<IPagedList<PostView>> GetPostsAsync(MaterialsShowOptions options);

		Task<IPagedList<PostView>> GetPostsFromMultiCategoriesAsync(MaterialsMultiCatShowOptions options);
	}

	public class BlogPresenter : DbService, IBlogPresenter
	{
		protected readonly IOptionsMonitor<BlogOptions> blogOptions;

		public BlogPresenter(
			DataBaseConnection db,
			IOptionsMonitor<BlogOptions> blogOptions) : base(db)
		{
			this.blogOptions = blogOptions;
		}

		public virtual async Task<IPagedList<PostView>> GetPostsAsync(MaterialsShowOptions options)
		{
			IQueryable<Material> query = db.Materials;

			if (!options.ShowHidden)
				query = query.Where(x => !x.IsHidden);

			if (!options.ShowDeleted)
				query = query.Where(x => x.DeletedDate == null);

			var rez = await query.GetPagedListAsync(
				x => new PostView
				{
					Id = x.Id,
					Title = x.Title,
					Preview = x.Text,
					CommentsCount = x.CommentsCount,
					AuthorName = x.Author.UserName,
					AuthorLink = x.Author.Link,
					AuthorAvatar = x.Author.Avatar,
					PublishDate = x.PublishDate,
					CategoryName = x.Category.Name,
					IsCommentsBlocked = x.IsCommentsBlocked,
					IsHidden = x.IsHidden,
					DeletedDate = x.DeletedDate
				},
				x => x.CategoryId == options.CategoryId,
				x => x.OrderByDescending(y => y.PublishDate),
				options.Page,
				options.PageSize);

			foreach (var postView in rez.Items)
			{
				var textLength = postView.Preview.Length;
				postView.Preview =
					MakePreview.HtmlFirstImage(new HtmlParser().Parse(postView.Preview),
						blogOptions.CurrentValue.PreviewLength);
				postView.HasMoreText = postView.Preview.Length != textLength;
			}

			return rez;
		}

		public virtual async Task<IPagedList<PostView>> GetPostsFromMultiCategoriesAsync(
			MaterialsMultiCatShowOptions options)
		{
			var rez = await db.MaterialsVisible.GetPagedListAsync(
				x => new PostView
				{
					Id = x.Id,
					Title = x.Title,
					Preview = x.Text,
					CommentsCount = x.CommentsCount,
					AuthorName = x.Author.UserName,
					AuthorLink = x.Author.Link,
					AuthorAvatar = x.Author.Avatar,
					PublishDate = x.PublishDate,
					CategoryName = x.Category.Name,
					CategoryTitle = x.Category.Title,
					IsCommentsBlocked = x.IsCommentsBlocked
				},
				x => options.CategoriesIds.Contains(x.CategoryId),
				x => x.OrderByDescending(y => y.PublishDate),
				options.Page,
				options.PageSize);

			foreach (var postView in rez.Items)
			{
				var textLength = postView.Preview.Length;
				postView.Preview =
					MakePreview.HtmlFirstImage(new HtmlParser().Parse(postView.Preview), options.PreviewSize);
				postView.HasMoreText = postView.Preview.Length != textLength;
			}

			return rez;
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