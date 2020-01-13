using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Presenters;
using SunEngine.Core.Security;
using SunEngine.Core.Utils.PagedList;

namespace SunEngine.Core.Controllers
{
	/// <summary>
	/// Get blog posts controller
	/// </summary>
	public class BlogController : BaseController
	{
		protected readonly OperationKeysContainer OperationKeys;

		protected readonly IOptionsMonitor<BlogOptions> blogOptions;
		protected readonly ICategoriesCache categoriesCache;
		protected readonly IAuthorizationService authorizationService;
		protected readonly IBlogPresenter blogPresenter;
		protected readonly IComponentsCache componentsCache;

		public BlogController(
			IOptionsMonitor<BlogOptions> blogOptions,
			IAuthorizationService authorizationService,
			ICategoriesCache categoriesCache,
			OperationKeysContainer operationKeysContainer,
			IBlogPresenter blogPresenter,
			IComponentsCache componentsCache,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			OperationKeys = operationKeysContainer;

			this.blogOptions = blogOptions;
			this.authorizationService = authorizationService;
			this.categoriesCache = categoriesCache;
			this.blogPresenter = blogPresenter;
			this.componentsCache = componentsCache;
		}

		[HttpPost]
		public virtual async Task<IActionResult> GetPosts(string categoryName, int page = 1, bool showDeleted = false)
		{
			var category = categoriesCache.GetCategory(categoryName);

			if (category == null)
				return BadRequest();


			if (!authorizationService.HasAccess(User.Roles, category, OperationKeys.MaterialAndCommentsRead))
				return Unauthorized();


			var options = new MaterialsShowOptions
			{
				CategoryId = category.Id,
				Page = page,
				PageSize = blogOptions.CurrentValue.PostsPageSize
			};

			if (authorizationService.HasAccess(User.Roles, category, OperationKeys.MaterialHide))
				options.ShowHidden = true;

			if (showDeleted && authorizationService.HasAccess(User.Roles, category, OperationKeys.MaterialDeleteAny))
				options.ShowDeleted = true;

			async Task<IPagedList<PostView>> LoadDataAsync()
			{
				return await blogPresenter.GetPostsAsync(options);
			}

			if (showDeleted)
				return Ok(await LoadDataAsync());

			return await CacheContentAsync(category, category.Id, LoadDataAsync, page);
		}

		[HttpPost]
		public virtual async Task<IActionResult> GetPostsFromMultiCategories(string componentName, int page = 1)
		{
			var component = componentsCache.GetComponentServerCached(componentName, User.Roles);
			if (component == null)
				return BadRequest($"No component {componentName} found in cache");

			PostsComponentData componentData = component.Data as PostsComponentData;

			var materialsCategoriesDic = categoriesCache.GetAllCategoriesWithChildren(componentData.CategoriesNames);

			IList<CategoryCached> categoriesList = authorizationService.GetAllowedCategories(User.Roles,
				materialsCategoriesDic.Values, OperationKeys.MaterialAndCommentsRead);

			if (categoriesList.Count == 0)
				return BadRequest("No categories to show");

			var categoriesIds = categoriesList.Select(x => x.Id);

			var options = new MaterialsMultiCatShowOptions
			{
				CategoriesIds = categoriesList.Select(x => x.Id),
				Page = page,
				PageSize = componentData.PageSize,
				PreviewSize = componentData.PreviewSize
			};

			async Task<IPagedList<PostView>> LoadDataAsync()
			{
				return await blogPresenter.GetPostsFromMultiCategoriesAsync(options);
			}

			return await CacheContentAsync(component, categoriesIds, LoadDataAsync, page);
		}
	}

	public class PostsComponentData
	{
		public string CategoriesNames { get; set; }
		public int PreviewSize { get; set; }
		public int PageSize { get; set; }
	}
}