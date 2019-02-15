using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SunEngine.Configuration.Options;
using SunEngine.Managers;
using SunEngine.Models;
using SunEngine.Presenters;
using SunEngine.Presenters.PagedList;
using SunEngine.Security.Authorization;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
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
            OperationKeysContainer operationKeysContainer,
            IForumPresenter forumPresenter,
            MyUserManager userManager,
            IRolesCache rolesCache) : base(rolesCache, userManager)
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
            Category categoryParent = categoriesCache.GetCategory(categoryName);

            if (categoryParent == null)
            {
                return BadRequest();
            }

            var allCategories = categoryParent.GetAllSubcategories().Values.Where(x=>!x.IsFolder);

            var categories =
                authorizationService.GetAllowedCategories(User.Roles, allCategories,
                    OperationKeys.MaterialAndMessagesRead);

            var categoriesIds = categories.Select(x => x.Id).ToArray();

            IPagedList<TopicInfoViewModel> topics = await forumPresenter.GetNewTopics(categoriesIds,
                page, forumOptions.NewTopicsPageSize, forumOptions.NewTopicsMaxPages);

            return Json(topics);
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetThread(string categoryName, int page = 1)
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

            IPagedList<TopicInfoViewModel> topics =
                await forumPresenter.GetThread(category.Id, page, forumOptions.ThreadMaterialsPageSize);

            return Json(topics);
        }
    }
}