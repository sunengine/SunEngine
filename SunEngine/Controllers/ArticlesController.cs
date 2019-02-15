using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using SunEngine.Configuration.Options;
using SunEngine.Managers;
using SunEngine.Models;
using SunEngine.Presenters;
using SunEngine.Presenters.PagedList;
using SunEngine.Security.Authorization;
using SunEngine.Stores;

namespace SunEngine.Controllers
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
            MyUserManager userManager,
            IRolesCache rolesCache) : base(rolesCache, userManager)
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
            Category category = categoriesCache.GetCategory(categoryName);

            if (category == null)
            {
                return BadRequest();
            }

            if (!authorizationService.HasAccess(User.Roles, category, OperationKeys.MaterialAndMessagesRead))
            {
                return Unauthorized();
            }

            IPagedList<ArticleInfoViewModel> articles =
                await articlesPresenter.GetArticlesAsync(category.Id, page, articlesOptions.CategoryPageSize);

            return Json(articles);
        }
    }
}