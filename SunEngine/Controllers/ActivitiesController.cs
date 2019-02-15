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
            IList<Category> materialsCategoriesList = new List<Category>();

            var materialsCategoriesDic = categoriesCache.GetAllCategoriesIncludeSub(materialsCategories);

            foreach (var cat in materialsCategoriesDic.Values) // TODO move to AuthorizationService
            {
                if (authorizationService.HasAccess(User.Roles, cat, OperationKeys.MaterialAndMessagesRead))
                {
                    materialsCategoriesList.Add(cat);
                }
            }
            
            IList<Category> messagesCategoriesList = new List<Category>();
            
            var messagesCategoriesDic = categoriesCache.GetAllCategoriesIncludeSub(messagesCategories);

            foreach (var cat in messagesCategoriesDic.Values)
            {
                if (authorizationService.HasAccess(User.Roles, cat, OperationKeys.MaterialAndMessagesRead))
                {
                    messagesCategoriesList.Add(cat);
                }
            }


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