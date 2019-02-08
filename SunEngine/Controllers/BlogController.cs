using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SunEngine.Commons.Models;
using SunEngine.Commons.PagedList;
using SunEngine.Commons.Services;
using SunEngine.Configuration.Options;
using SunEngine.Presenters;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
    public class BlogController : BaseController
    {
        private readonly BlogOptions blogOptions;
        private readonly ICategoriesStore categoriesStore;
        private readonly OperationKeysContainer OperationKeys;
        private readonly IAuthorizationService authorizationService;
        private readonly BlogPresenter blogPresenter;


        public BlogController(IOptions<BlogOptions> blogOptions,
            IAuthorizationService authorizationService,
            ICategoriesStore categoriesStore,
            OperationKeysContainer operationKeysContainer,
            BlogPresenter blogPresenter,
            MyUserManager userManager,
            IUserGroupStore userGroupStore) : base(userGroupStore, userManager)
        {
            this.OperationKeys = operationKeysContainer;

            this.blogOptions = blogOptions.Value;
            this.authorizationService = authorizationService;
            this.categoriesStore = categoriesStore;
            this.blogPresenter = blogPresenter;
        }

        [HttpPost]
        public async Task<IActionResult> GetPosts(string categoryName, int page = 1)
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