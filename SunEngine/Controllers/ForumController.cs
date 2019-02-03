using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SunEngine.Authorization.ControllersAuthorization;
using SunEngine.Commons.Models;
using SunEngine.Commons.PagedList;
using SunEngine.Commons.Services;
using SunEngine.EntityServices;
using SunEngine.Options;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
    public class ForumController : BaseController
    {
        private readonly OperationKeysContainer OperationKeys;

        private readonly ForumOptions forumOptions;
        private readonly ICategoriesStore categoriesStore;
        private readonly CategoriesAuthorization categoriesAuthorization;
        private readonly IAuthorizationService authorizationService;
        private readonly ForumService forumService;


        public ForumController(IOptions<ForumOptions> forumOptions,
            IAuthorizationService authorizationService,
            ICategoriesStore categoriesStore,
            CategoriesAuthorization categoriesAuthorization,
            OperationKeysContainer operationKeysContainer,
            ForumService forumService,
            MyUserManager userManager,
            IUserGroupStore userGroupStore) : base(userGroupStore, userManager)
        {
            this.OperationKeys = operationKeysContainer;

            this.forumService = forumService;
            this.forumOptions = forumOptions.Value;
            this.categoriesAuthorization = categoriesAuthorization;
            this.authorizationService = authorizationService;
            this.categoriesStore = categoriesStore;
        }

        [HttpPost]
        public async Task<IActionResult> GetNewTopics(string categoryName, int page = 1)
        {
            Category categoryParent = categoriesStore.GetCategory(categoryName);

            if (categoryParent == null)
            {
                return BadRequest();
            }

            List<int> categoriesIds =
                categoriesAuthorization.GetSubCategoriesIdsCanRead(User.UserGroups, categoryParent);

            IPagedList<TopicInfoViewModel> topics = await forumService.GetNewTopics(categoriesIds,
                page, forumOptions.NewTopicsPageSize, forumOptions.NewTopicsMaxPages);

            return Json(topics);
        }

        [HttpPost]
        public async Task<IActionResult> GetThread(string categoryName, int page = 1)
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

            IPagedList<TopicInfoViewModel> topics =
                await forumService.GetThread(category.Id, page, forumOptions.ThreadMaterialsPageSize);

            return Json(topics);
        }
    }
}