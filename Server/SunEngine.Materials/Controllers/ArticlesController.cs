using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Controllers;
using SunEngine.Core.Managers;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Security;
using SunEngine.Core.Utils.PagedList;
using SunEngine.Materials.Presenters;

namespace SunEngine.Materials.Controllers
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

		protected readonly IMaterialsManager materialManager;
		protected readonly MaterialsAuthorization materialsAuthorization;

		public ArticlesController(
			IOptionsMonitor<ArticlesOptions> articlesOptions,
			IAuthorizationService authorizationService,
			ICategoriesCache categoriesCache,
			OperationKeysContainer operationKeysContainer,
			IArticlesPresenter articlesPresenter,
			IMaterialsManager materialManager,
			MaterialsAuthorization materialsAuthorization,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			OperationKeys = operationKeysContainer;

			this.articlesOptions = articlesOptions;
			this.authorizationService = authorizationService;
			this.categoriesCache = categoriesCache;
			this.articlesPresenter = articlesPresenter;
			this.materialManager = materialManager;
			this.materialsAuthorization = materialsAuthorization;
		}


		[HttpPost]
		public virtual async Task<IActionResult> GetArticles(
			string categoryName, int page = 1, bool showDeleted = false)
		{
			CategoryCached category = categoriesCache.GetCategory(categoryName);

			if (category == null)
				return BadRequest();

			if (!authorizationService.HasAccess(User.Roles, category, OperationKeys.MaterialAndCommentsRead))
				return Unauthorized();

			var options = new MaterialsShowOptions
			{
				CategoryId = category.Id,
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

			if (showDeleted || options.ShowHidden)
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

			var options = new MaterialsShowOptions
			{
				CategoriesIds = categoriesList.Select(x => x.Id),
				Page = page,
				PageSize = articlesOptions.CurrentValue.CategoryPageSize
			};

			IPagedList<ArticleInfoView> articles = await articlesPresenter.GetArticlesFromMultiCategoriesAsync(options);

			return Json(articles);
		}

		/// <summary>
		/// Move article down in sort order 
		/// </summary>
		[HttpPost]
		public async Task<IActionResult> Down(int id, int countMove = 1)
		{
			Material material = await materialManager.GetAsync(id);
			if (material != null)
			{
				if (countMove <= 0 || material.SortNumber - countMove < 1)
					return BadRequest("Incorrect count move");
				if (await materialsAuthorization.CanUpdateAsync(User, material))
				{
					try
					{
						await materialManager.DownAsync(id);
						return Ok();
					}
					catch
					{
					}
				}
				else
					return Forbid();
			}
      return BadRequest("Invalid article ID");
		}

		public async Task<IActionResult> Up(int id, int countMove = 1)
		{
			if (countMove < 1)
				return BadRequest("Incorrect count move");
			Material material = await materialManager.GetAsync(id);
			if (material != null)
			{
				if (await materialsAuthorization.CanUpdateAsync(User, material))
				{
					try
					{
						await materialManager.UpAsync(id, countMove);
						return Ok();
					}
					catch
					{
					}
				}
				else
					return Forbid();
			}

			return BadRequest("Invalid article ID");
		}
	}
}
