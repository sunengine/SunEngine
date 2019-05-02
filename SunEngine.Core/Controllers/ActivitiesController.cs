using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Presenters;
using SunEngine.Core.Security;

namespace SunEngine.Core.Controllers
{
    /// <summary>
    /// Whats new activities on site, new materials and comments
    /// </summary>
    public class ActivitiesController : BaseController
    {
        protected const int MaxActivitiesInQuery = 50;

        protected readonly OperationKeysContainer OperationKeys;
        protected readonly IAuthorizationService authorizationService;
        protected readonly ICategoriesCache categoriesCache;
        protected readonly IActivitiesPresenter activitiesPresenter;

        public ActivitiesController(
            OperationKeysContainer operationKeysContainer,
            ICategoriesCache categoriesCache,
            IAuthorizationService authorizationService,
            IActivitiesPresenter activitiesPresenter,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            OperationKeys = operationKeysContainer;
            this.categoriesCache = categoriesCache;
            this.authorizationService = authorizationService;
            this.activitiesPresenter = activitiesPresenter;
        }

        public async Task<IActionResult> GetActivities(string materialsCategories, string commentsCategories,
            int number)
        {
            var materialsCategoriesDic = categoriesCache.GetAllCategoriesIncludeSub(materialsCategories);

            IList<CategoryCached> materialsCategoriesList = authorizationService.GetAllowedCategories(User.Roles, materialsCategoriesDic.Values,
                OperationKeys.MaterialAndCommentsRead);
            
            
            var commentsCategoriesDic = categoriesCache.GetAllCategoriesIncludeSub(commentsCategories);

            IList<CategoryCached> commentsCategoriesList = authorizationService.GetAllowedCategories(User.Roles, commentsCategoriesDic.Values,
                OperationKeys.MaterialAndCommentsRead);


            int[] materialsCategoriesIds = materialsCategoriesList.Select(x => x.Id).ToArray();
            int[] commentsCategoriesIds = commentsCategoriesList.Select(x => x.Id).ToArray();

            if (number > MaxActivitiesInQuery)
                number = MaxActivitiesInQuery;

            var rez = await activitiesPresenter.GetActivitiesAsync(materialsCategoriesIds, commentsCategoriesIds,
                number);
            return Ok(rez);
        }
    }
}