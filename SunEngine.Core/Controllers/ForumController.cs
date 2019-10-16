using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Presenters;
using SunEngine.Core.Security;
using SunEngine.Core.Utils.PagedList;

namespace SunEngine.Core.Controllers
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


        public ForumController(
            IOptions<ForumOptions> forumOptions,
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

            var categoriesIds = categories.Select(x => x.Id);

            var options = new MaterialsMultiCatShowOptions
            {
                CategoriesIds = categoriesIds,
                Page = page,
                PageSize = forumOptions.NewTopicsPageSize
            };

            async Task<IPagedList<TopicInfoView>> LoadDataAsync()
            {
                return await forumPresenter.GetNewTopicsAsync(options, forumOptions.NewTopicsMaxPages);
            }

            return await CacheContentAsync(categoryParent, categoriesIds, LoadDataAsync, page);
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetThread(string categoryName, int page = 1, bool showDeleted = false)
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
                PageSize = forumOptions.ThreadMaterialsPageSize
            };

            if (authorizationService.HasAccess(User.Roles, category, OperationKeys.MaterialHide))
                options.ShowHidden = true;

            if (showDeleted && authorizationService.HasAccess(User.Roles, category, OperationKeys.MaterialDeleteAny))
                options.ShowDeleted = true;

            async Task<IPagedList<TopicInfoView>> LoadDataAsync()
            {
                return await forumPresenter.GetThreadAsync(options);
            }

            return await CacheContentAsync(category, category.Id, LoadDataAsync, page);
        }
    }
}
