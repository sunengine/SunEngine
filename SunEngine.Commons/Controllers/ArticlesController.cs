using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SunEngine.Commons.Cache;
using SunEngine.Commons.Cache.CacheModels;
using SunEngine.Commons.Configuration.Options;
using SunEngine.Commons.Presenters;
using SunEngine.Commons.Security;
using SunEngine.Commons.Utils.PagedList;

namespace SunEngine.Commons.Controllers
{
    public class ArticlesController : BaseController
    {
        protected readonly OperationKeysContainer OperationKeys;

        protected readonly ArticlesOptions articlesOptions;
        protected readonly ICategoriesCache categoriesCache;
        protected readonly IAuthorizationService authorizationService;

        protected readonly IArticlesPresenter articlesPresenter;


        public ArticlesController(
            IOptions<ArticlesOptions> articlesOptions,
            IAuthorizationService authorizationService,
            ICategoriesCache categoriesCache,
            OperationKeysContainer operationKeysContainer,
            IArticlesPresenter articlesPresenter,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            OperationKeys = operationKeysContainer;

            this.articlesOptions = articlesOptions.Value;
            this.authorizationService = authorizationService;
            this.categoriesCache = categoriesCache;
            this.articlesPresenter = articlesPresenter;
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetArticles(string categoryName, int page = 1)
        {
            CategoryCached category = categoriesCache.GetCategory(categoryName);

            if (category == null)
                return BadRequest();

            if (!authorizationService.HasAccess(User.Roles, category, OperationKeys.MaterialAndCommentsRead))
                return Unauthorized();

            async Task<IPagedList<ArticleInfoView>> LoadDataAsync()
            {
                return await articlesPresenter.GetArticlesAsync(category.Id, page, articlesOptions.CategoryPageSize);   
            }

            return await CacheContentAsync(category, category.Id, LoadDataAsync, page);
        }
        
        [HttpPost]
        public virtual async Task<IActionResult> GetArticlesFromMultiCategories(string categoriesNames, int page = 1)
        {
            var materialsCategoriesDic = categoriesCache.GetAllCategoriesIncludeSub(categoriesNames);

            IList<CategoryCached> categoriesList = authorizationService.GetAllowedCategories(User.Roles, materialsCategoriesDic.Values,
                OperationKeys.MaterialAndCommentsRead);

            if (categoriesList.Count == 0)
                return BadRequest("No categories to show");

            var categoriesIds = categoriesList.Select(x => x.Id).ToArray();
            
            IPagedList<ArticleInfoView> articles =
                await articlesPresenter.GetArticlesFromMultiCategoriesAsync(categoriesIds, page, articlesOptions.CategoryPageSize);

            return Json(articles);
        }
    }
}