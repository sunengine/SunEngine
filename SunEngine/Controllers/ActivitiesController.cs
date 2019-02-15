using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Managers;
using SunEngine.Models;
using SunEngine.Presenters;
using SunEngine.Security.Authorization;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
    public class ActivitiesController : BaseController
    {
        protected const int MaxActivitiesInQuery = 50;

        protected readonly OperationKeysContainer OperationKeys;
        protected readonly IAuthorizationService authorizationService;
        protected readonly ICategoriesCache categoriesCache;
        protected readonly IActivitiesPresenter activitiesPresenter;

        public ActivitiesController(
            OperationKeysContainer operationKeysContainer,
            IRolesCache rolesCache,
            ICategoriesCache categoriesCache,
            IAuthorizationService authorizationService,
            IActivitiesPresenter activitiesPresenter,
            MyUserManager userManager) : base(rolesCache, userManager)
        {
            OperationKeys = operationKeysContainer;
            this.categoriesCache = categoriesCache;
            this.authorizationService = authorizationService;
            this.activitiesPresenter = activitiesPresenter;
        }

        public async Task<IActionResult> GetActivities(string materialsCategories, string messagesCategories,
            int number)
        {
            var materialsCategoriesDic = categoriesCache.GetAllCategoriesIncludeSub(materialsCategories);

            IList<Category> materialsCategoriesList = authorizationService.GetAllowedCategories(User.Roles, materialsCategoriesDic.Values,
                OperationKeys.MaterialAndMessagesRead);
            
            
            var messagesCategoriesDic = categoriesCache.GetAllCategoriesIncludeSub(messagesCategories);

            IList<Category> messagesCategoriesList = authorizationService.GetAllowedCategories(User.Roles, messagesCategoriesDic.Values,
                OperationKeys.MaterialAndMessagesRead);


            int[] materialsCategoriesIds = materialsCategoriesList.Select(x => x.Id).ToArray();
            int[] messagesCategoriesIds = messagesCategoriesList.Select(x => x.Id).ToArray();

            if (number > MaxActivitiesInQuery)
                number = MaxActivitiesInQuery;

            var rez = await activitiesPresenter.GetActivitiesAsync(materialsCategoriesIds, messagesCategoriesIds,
                number);
            return Ok(rez);
        }
    }
}