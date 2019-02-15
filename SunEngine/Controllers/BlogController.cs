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
    public class BlogController : BaseController
    {
        protected readonly BlogOptions blogOptions;
        protected readonly ICategoriesCache categoriesCache;
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
            this.categoriesCache = categoriesCache;
            this.blogPresenter = blogPresenter;
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetPosts(string categoryName, int page = 1)
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

            IPagedList<PostViewModel> posts =
                await blogPresenter.GetPostsAsync(category.Id, page, blogOptions.PostsPageSize);

            return Json(posts);
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetCategoriesPosts(string categoriesNames, int page = 1)
        {
            var names = categoriesNames.Split(',').Select(x => x.Trim()).ToList();

            List<Category> categories = new List<Category>();
            foreach (var categoryName in names)
            {
                var category = categoriesCache.GetCategory(categoryName);
                if (category != null && authorizationService.HasAccess(User.Roles, category, OperationKeys.MaterialAndMessagesRead))
                {
                    categories.Add(category);
                }
            }

            if (categories.Count == 0)
            {
                return BadRequest("No categories to show");
            }

            var categoriesIds = categories.Select(x => x.Id).ToArray();

            IPagedList<PostViewModel> posts =
                await blogPresenter.GetCategoriesPostsAsync(categoriesIds, page, blogOptions.PostsPageSize);

            return Json(posts);
        }
    }
}