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
        protected readonly ICategoriesCache CategoriesCache;
        protected readonly OperationKeysContainer OperationKeys;
        protected readonly IAuthorizationService authorizationService;
        protected readonly IBlogPresenter blogPresenter;


        public BlogController(IOptions<BlogOptions> blogOptions,
            IAuthorizationService authorizationService,
            ICategoriesCache categoriesCache,
            OperationKeysContainer operationKeysContainer,
            IBlogPresenter blogPresenter,
            MyUserManager userManager,
            IRolesCache rolesCache) : base(rolesCache, userManager)
        {
            OperationKeys = operationKeysContainer;

            this.blogOptions = blogOptions.Value;
            this.authorizationService = authorizationService;
            this.CategoriesCache = categoriesCache;
            this.blogPresenter = blogPresenter;
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetPosts(string categoryName, int page = 1)
        {
            Category category = CategoriesCache.GetCategory(categoryName);

            if (category == null)
            {
                return BadRequest();
            }            
            
            if (!authorizationService.HasAccess(User.Roles, category, OperationKeys.MaterialAndMessagesRead))
            {
                return Unauthorized();
            }

            IPagedList<PostViewModel> posts =
                await blogPresenter.GetPostsAsync(category.Id, page,blogOptions.PostsPageSize);

            return Json(posts);
        }
    }

    
}