using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SunEngine.Configuration.Options;
using SunEngine.Managers;
using SunEngine.Models;
using SunEngine.Presenters;
using SunEngine.Presenters.PagedList;
using SunEngine.Security.Authorization;
using SunEngine.Stores;
using SunEngine.Utils;

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
            {
                return BadRequest();
            }

            var allCategories = categoryParent.AllSubCategories.Where(x => x.IsMaterialsContainer);

            var categories =
                authorizationService.GetAllowedCategories(User.Roles, allCategories,
                    OperationKeys.MaterialAndCommentsRead);

            var categoriesIds = categories.Select(x => x.Id).ToArray();

            async Task<IPagedList<TopicInfoViewModel>> LoadDataAsync()
            {
                return await forumPresenter.GetNewTopics(categoriesIds,
                    page, forumOptions.NewTopicsPageSize, forumOptions.NewTopicsMaxPages);
            }

            return await CacheContentAsync(categoryParent, categoriesIds, LoadDataAsync);
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetThread(string categoryName, int page = 1)
        {
            var category = categoriesCache.GetCategory(categoryName);

            if (category == null)
            {
                return BadRequest();
            }

            if (!authorizationService.HasAccess(User.Roles, category, OperationKeys.MaterialAndCommentsRead))
            {
                return Unauthorized();
            }

            async Task<IPagedList<TopicInfoViewModel>> LoadDataAsync()
            {
                return await forumPresenter.GetThread(category.Id, page, forumOptions.ThreadMaterialsPageSize);
            }

            return await CacheContentAsync(category, category.Id, LoadDataAsync);
        }
    }
}