using System;
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
        protected readonly ICategoriesStore categoriesStore;
        protected readonly IAuthorizationService authorizationService;

        protected readonly IArticlesPresenter articlesPresenter;


        public ArticlesController(
            IOptions<ArticlesOptions> articlesOptions,
            IAuthorizationService authorizationService,
            ICategoriesStore categoriesStore,
            OperationKeysContainer operationKeysContainer,
            IArticlesPresenter articlesPresenter,
            MyUserManager userManager,
            IUserGroupStore userGroupStore) : base(userGroupStore, userManager)
        {
            OperationKeys = operationKeysContainer;

            this.articlesOptions = articlesOptions.Value;
            this.authorizationService = authorizationService;
            this.categoriesStore = categoriesStore;
            this.articlesPresenter = articlesPresenter;
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetArticles(string categoryName, int page = 1)
        {
            Category category = categoriesStore.GetCategory(categoryName);

            if (category == null)
            {
                return BadRequest();
            }

            if (!authorizationService.HasAccess(User.UserGroups, category, OperationKeys.MaterialAndMessagesRead))
            {
                return Unauthorized();
            }

            IPagedList<ArticleInfoViewModel> articles =
                await articlesPresenter.GetArticlesAsync(category.Id, page, articlesOptions.ArticlesCategoryPageSize);

            return Json(articles);
        }
    }
}