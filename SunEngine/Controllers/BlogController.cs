using System;
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
    public class BlogController : BaseController
    {
        protected readonly BlogOptions blogOptions;
        protected readonly ICategoriesStore categoriesStore;
        protected readonly OperationKeysContainer OperationKeys;
        protected readonly IAuthorizationService authorizationService;
        protected readonly BlogPresenter blogPresenter;


        public BlogController(IOptions<BlogOptions> blogOptions,
            IAuthorizationService authorizationService,
            ICategoriesStore categoriesStore,
            OperationKeysContainer operationKeysContainer,
            BlogPresenter blogPresenter,
            MyUserManager userManager,
            IUserGroupStore userGroupStore) : base(userGroupStore, userManager)
        {
            OperationKeys = operationKeysContainer;

            this.blogOptions = blogOptions.Value;
            this.authorizationService = authorizationService;
            this.categoriesStore = categoriesStore;
            this.blogPresenter = blogPresenter;
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetPosts(string categoryName, int page = 1)
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

            IPagedList<PostViewModel> posts =
                await blogPresenter.GetPostsAsync(category.Id, page,blogOptions.PostsPageSize);

            return Json(posts);
        }
    }

    
}