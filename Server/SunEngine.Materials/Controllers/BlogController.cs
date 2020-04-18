using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Controllers;
using SunEngine.Core.Sections;
using SunEngine.Core.Security;
using SunEngine.Core.Utils.PagedList;
using SunEngine.Materials.Presenters;

namespace SunEngine.Materials.Controllers
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
		protected readonly ISectionsCache sectionsCache;

		public BlogController(
			IOptionsMonitor<BlogOptions> blogOptions,
			IAuthorizationService authorizationService,
			ICategoriesCache categoriesCache,
			OperationKeysContainer operationKeysContainer,
			IBlogPresenter blogPresenter,
			ISectionsCache sectionsCache,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			OperationKeys = operationKeysContainer;

			this.blogOptions = blogOptions;
			this.authorizationService = authorizationService;
			this.categoriesCache = categoriesCache;
			this.blogPresenter = blogPresenter;
			this.sectionsCache = sectionsCache;
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

			if (showDeleted)
				return Ok(await LoadDataAsync());

			return await CacheContentAsync(category, category.Id, LoadDataAsync, page);


			async Task<IPagedList<PostView>> LoadDataAsync() => await blogPresenter.GetPostsAsync(options);
		}

		[HttpPost]
		public virtual async Task<IActionResult> GetPostsFromMultiCategories(string sectionName, int page = 1)
		{
			var section = sectionsCache.GetSectionServerCached(sectionName, User.Roles);
			if (section == null)
				return BadRequest($"No component {sectionName} found in cache");

			PostsServerSection sectionData = section.GetData<PostsServerSection>();

			var materialsCategoriesDic = categoriesCache.GetAllCategoriesWithChildren(sectionData.Categories);
			var materialsCategoriesExcludeDic =
				categoriesCache.GetAllCategoriesWithChildren(sectionData.CategoriesExclude);

			foreach (var (key, _) in materialsCategoriesExcludeDic)
				materialsCategoriesDic.Remove(key);

			var categoriesList = authorizationService.GetAllowedCategories(User.Roles,
				materialsCategoriesDic.Values, OperationKeys.MaterialAndCommentsRead);

			if (categoriesList.Count == 0)
				return BadRequest("No categories to show");

			var categoriesIds = categoriesList.Select(x => x.Id);

			var options = new MaterialsShowOptions 
			{
				CategoriesIds = categoriesList.Select(x => x.Id),
				Page = page,
				PageSize = sectionData.PageSize,
				PreviewSize = sectionData.PreviewSize
			};

			return await CacheContentAsync(section, categoriesIds,
				() => blogPresenter.GetPostsFromMultiCategoriesAsync(options), page);
		}
	}
}