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
		protected const int MaxActivitiesInQuery = 300;

		protected readonly OperationKeysContainer OperationKeys;
		protected readonly IAuthorizationService authorizationService;
		protected readonly ICategoriesCache categoriesCache;
		protected readonly IActivitiesPresenter activitiesPresenter;
		protected readonly IComponentsCache componentsCache;

		public ActivitiesController(
			OperationKeysContainer operationKeysContainer,
			ICategoriesCache categoriesCache,
			IAuthorizationService authorizationService,
			IActivitiesPresenter activitiesPresenter,
			IComponentsCache componentsCache,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			OperationKeys = operationKeysContainer;
			this.categoriesCache = categoriesCache;
			this.authorizationService = authorizationService;
			this.activitiesPresenter = activitiesPresenter;
			this.componentsCache = componentsCache;
		}

		public async Task<IActionResult> GetActivities(string componentName)
		{
			var component = componentsCache.GetComponentServerCached(componentName, User.Roles);
			if (component == null)
				return BadRequest($"No component {componentName} found in cache");

			ActivitiesComponentData componentData = component.Data as ActivitiesComponentData;

			var materialsCategoriesDic =
				categoriesCache.GetAllCategoriesWithChildren(componentData.MaterialsCategories);

			IList<CategoryCached> materialsCategoriesList = authorizationService.GetAllowedCategories(User.Roles,
				materialsCategoriesDic.Values, OperationKeys.MaterialAndCommentsRead);


			var commentsCategoriesDic = categoriesCache.GetAllCategoriesWithChildren(componentData.CommentsCategories);

			IList<CategoryCached> commentsCategoriesList = authorizationService.GetAllowedCategories(User.Roles,
				commentsCategoriesDic.Values, OperationKeys.MaterialAndCommentsRead);


			int[] materialsCategoriesIds = materialsCategoriesList.Select(x => x.Id).ToArray();
			int[] commentsCategoriesIds = commentsCategoriesList.Select(x => x.Id).ToArray();

			int number = componentData.Number;

			if (number > MaxActivitiesInQuery)
				number = MaxActivitiesInQuery;

			async Task<ActivityView[]> LoadDataAsync()
			{
				return await activitiesPresenter.GetActivitiesAsync(materialsCategoriesIds, commentsCategoriesIds,
					number);
			}

			return await CacheContentAsync(
				component,
				materialsCategoriesIds.Union(commentsCategoriesIds),
				LoadDataAsync);
		}
	}

	public class ActivitiesComponentData
	{
		public string MaterialsCategories { get; set; }
		public string CommentsCategories { get; set; }
		public int Number { get; set; }
	}
}