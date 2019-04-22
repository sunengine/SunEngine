using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SunEngine.Commons.Cache.Services;
using SunEngine.Commons.Configuration.Options;
using SunEngine.Commons.Presenters;
using SunEngine.Commons.Security;
using SunEngine.Commons.Utils.PagedList;

namespace SunEngine.Commons.Controllers
{
    /// <summary>
    /// Get new forum topics controller
    /// </summary>
    public class ForumController : BaseController
    {
        protected readonly OperationKeysContainer OperationKeys;
        protected readonly ForumOptions forumOptions;
        protected readonly ICategoriesCache categoriesCache;
        protected readonly IAuthorizationService authorizationService;
        protected readonly IForumPresenter forumPresenter;


        public ForumController(IOptions<ForumOptions> forumOptions,
            IAuthorizationService authorizationService,
            ICategoriesCache categoriesCache,
            IContentCache contentCache,
            OperationKeysContainer operationKeysContainer,
            IForumPresenter forumPresenter,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            OperationKeys = operationKeysContainer;

            this.forumPresenter = forumPresenter;
            this.forumOptions = forumOptions.Value;
            this.authorizationService = authorizationService;
            this.categoriesCache = categoriesCache;
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetNewTopics(string categoryName, int page = 1)
        {
            var categoryParent = categoriesCache.GetCategory(categoryName);

            if (categoryParent == null)
                return BadRequest();

            var allCategories = categoryParent.AllSubCategories.Where(x => x.IsMaterialsContainer);

            var categories =
                authorizationService.GetAllowedCategories(User.Roles, allCategories,
                    OperationKeys.MaterialAndCommentsRead);

            var categoriesIds = categories.Select(x => x.Id).ToArray();

            async Task<IPagedList<TopicInfoView>> LoadDataAsync()
            {
                return await forumPresenter.GetNewTopics(categoriesIds,
                    page, forumOptions.NewTopicsPageSize, forumOptions.NewTopicsMaxPages);
            }

            return await CacheContentAsync(categoryParent, categoriesIds, LoadDataAsync, page);
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetThread(string categoryName, int page = 1)
        {
            var category = categoriesCache.GetCategory(categoryName);

            if (category == null)
                return BadRequest();

            if (!authorizationService.HasAccess(User.Roles, category, OperationKeys.MaterialAndCommentsRead))
                return Unauthorized();

            async Task<IPagedList<TopicInfoView>> LoadDataAsync()
            {
                return await forumPresenter.GetThread(category.Id, page, forumOptions.ThreadMaterialsPageSize);
            }

            return await CacheContentAsync(category, category.Id, LoadDataAsync, page);
        }
    }
}