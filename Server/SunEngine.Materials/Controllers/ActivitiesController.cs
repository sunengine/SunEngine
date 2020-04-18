using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Presenters;
using SunEngine.Core.Sections;
using SunEngine.Core.Security;
using SunEngine.Materials.Presenters;

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
		protected readonly ISectionsCache SectionsCache;

		public ActivitiesController(
			OperationKeysContainer operationKeysContainer,
			ICategoriesCache categoriesCache,
			IAuthorizationService authorizationService,
			IActivitiesPresenter activitiesPresenter,
			ISectionsCache SectionsCache,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			OperationKeys = operationKeysContainer;
			this.categoriesCache = categoriesCache;
			this.authorizationService = authorizationService;
			this.activitiesPresenter = activitiesPresenter;
			this.SectionsCache = SectionsCache;
		}

		public async Task<IActionResult> GetActivities(string sectionName)
		{
			var section = SectionsCache.GetSectionServerCached(sectionName, User.Roles);
			if (section == null)
				return BadRequest($"No component {sectionName} found in cache");

			ActivitiesServerSection sectionData = section.GetData<ActivitiesServerSection>();

			var materialsCategoriesDic =
				categoriesCache.GetAllCategoriesWithChildren(sectionData.MaterialsCategories);
			var materialsCategoriesExcludeDic =
				categoriesCache.GetAllCategoriesWithChildren(sectionData.MaterialsCategoriesExclude);

			foreach (var (key,_) in materialsCategoriesExcludeDic)
				materialsCategoriesDic.Remove(key);
			
			IList<CategoryCached> materialsCategoriesList = authorizationService.GetAllowedCategories(User.Roles,
				materialsCategoriesDic.Values, OperationKeys.MaterialAndCommentsRead);
			
			var commentsCategoriesDic = categoriesCache.GetAllCategoriesWithChildren(sectionData.CommentsCategories);
			var commentsCategoriesExcludeDic = categoriesCache.GetAllCategoriesWithChildren(sectionData.CommentsCategoriesExclude);

			foreach (var (key,_) in commentsCategoriesExcludeDic)
				commentsCategoriesDic.Remove(key);
			
			IList<CategoryCached> commentsCategoriesList = authorizationService.GetAllowedCategories(User.Roles,
				commentsCategoriesDic.Values, OperationKeys.MaterialAndCommentsRead);


			int[] materialsCategoriesIds = materialsCategoriesList.Select(x => x.Id).ToArray();
			int[] commentsCategoriesIds = commentsCategoriesList.Select(x => x.Id).ToArray();

			int number = sectionData.Number;

			if (number > MaxActivitiesInQuery)
				number = MaxActivitiesInQuery;

			async Task<ActivityView[]> LoadDataAsync()
			{
				return await activitiesPresenter.GetActivitiesAsync(materialsCategoriesIds, commentsCategoriesIds,
					number);
			}

			return await CacheContentAsync(
				section,
				materialsCategoriesIds.Union(commentsCategoriesIds),
				LoadDataAsync);
		}
	}
}