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
	/// Get article materials controller
	/// </summary>
	public class ArticlesController : BaseController
	{
		protected readonly OperationKeysContainer OperationKeys;

		protected readonly IOptionsMonitor<ArticlesOptions> articlesOptions;
		protected readonly ICategoriesCache categoriesCache;
		protected readonly IAuthorizationService authorizationService;

		protected readonly IArticlesPresenter articlesPresenter;


		public ArticlesController(
			IOptionsMonitor<ArticlesOptions> articlesOptions,
			IAuthorizationService authorizationService,
			ICategoriesCache categoriesCache,
			OperationKeysContainer operationKeysContainer,
			IArticlesPresenter articlesPresenter,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			OperationKeys = operationKeysContainer;

			this.articlesOptions = articlesOptions;
			this.authorizationService = authorizationService;
			this.categoriesCache = categoriesCache;
			this.articlesPresenter = articlesPresenter;
		}

		[HttpPost]
		public virtual async Task<IActionResult> GetArticles(
			string categoryName, OrderType sort = OrderType.PublishDate, int page = 1, bool showDeleted = false)
		{
			CategoryCached category = categoriesCache.GetCategory(categoryName);

			if (category == null)
				return BadRequest();

			if (!authorizationService.HasAccess(User.Roles, category, OperationKeys.MaterialAndCommentsRead))
				return Unauthorized();

			var options = new MaterialsShowOptions
			{
				CategoryId = category.Id,
				orderType = sort,
				Page = page,
				PageSize = articlesOptions.CurrentValue.CategoryPageSize
			};

			if (authorizationService.HasAccess(User.Roles, category, OperationKeys.MaterialHide))
				options.ShowHidden = true;

			if (showDeleted && authorizationService.HasAccess(User.Roles, category, OperationKeys.MaterialDeleteAny))
				options.ShowDeleted = true;


			async Task<IPagedList<ArticleInfoView>> LoadDataAsync()
			{
				return await articlesPresenter.GetArticlesAsync(options);
			}

			if (showDeleted)
				return Ok(await LoadDataAsync());

			return await CacheContentAsync(category, category.Id, LoadDataAsync, page);
		}

		[HttpPost]
		public virtual async Task<IActionResult> GetArticlesFromMultiCategories(string categoriesNames, int page = 1)
		{
			var materialsCategoriesDic = categoriesCache.GetAllCategoriesWithChildren(categoriesNames);

			IList<CategoryCached> categoriesList = authorizationService.GetAllowedCategories(User.Roles,
				materialsCategoriesDic.Values, OperationKeys.MaterialAndCommentsRead);

			if (categoriesList.Count == 0)
				return BadRequest("No categories to show");

			var options = new MaterialsMultiCatShowOptions
			{
				CategoriesIds = categoriesList.Select(x => x.Id),
				Page = page,
				PageSize = articlesOptions.CurrentValue.CategoryPageSize
			};

			IPagedList<ArticleInfoView> articles = await articlesPresenter.GetArticlesFromMultiCategoriesAsync(options);

			return Json(articles);
		}
	}
}