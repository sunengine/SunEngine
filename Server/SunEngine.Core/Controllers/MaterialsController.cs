using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Cache.Services.Counters;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors;
using SunEngine.Core.Errors.Exceptions;
using SunEngine.Core.Filters;
using SunEngine.Core.Managers;
using SunEngine.Core.Models.Authorization;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Presenters;
using SunEngine.Core.SectionsData;
using SunEngine.Core.Security;
using SunEngine.Core.Services;

namespace SunEngine.Core.Controllers
{
	/// <summary>
	/// Materials CRUD controller.
	/// Used by Articles, Forum and Blog
	/// </summary>
	public class MaterialsController : BaseController
	{
		protected readonly MaterialsAuthorization materialsAuthorization;
		protected readonly ICategoriesCache categoriesCache;
		protected readonly IMaterialsManager materialsManager;
		protected readonly IMaterialsPresenter materialsPresenter;
		protected readonly IMaterialsVisitsCounterCache materialsVisitsCounterCache;
		protected readonly ISectionsCache sectionsCache;
		protected readonly IServiceProvider serviceProvider;
		protected readonly OperationKeysContainer operationKeysContainer;
		protected readonly IAuthorizationService authorizationService;

		public MaterialsController(
			MaterialsAuthorization materialsAuthorization,
			ICategoriesCache categoriesCache,
			IMaterialsManager materialsManager,
			IMaterialsPresenter materialsPresenter,
			IMaterialsVisitsCounterCache materialsVisitsCounterCache,
			ISectionsCache sectionsCache,
			IServiceProvider serviceProvider,
			OperationKeysContainer operationKeysContainer,
			IAuthorizationService authorizationService) : base(serviceProvider)
		{
			this.materialsAuthorization = materialsAuthorization;
			this.categoriesCache = categoriesCache;
			this.materialsManager = materialsManager;
			this.materialsPresenter = materialsPresenter;
			this.materialsVisitsCounterCache = materialsVisitsCounterCache;
			this.sectionsCache = sectionsCache;
			this.serviceProvider = serviceProvider;
			this.operationKeysContainer = operationKeysContainer;
			this.authorizationService = authorizationService;
		}

		[HttpPost]
		public virtual async Task<IActionResult> Get(string idOrName)
		{
			if (int.TryParse(idOrName, out int id))
				return Get(await materialsPresenter.GetAsync(id));
			else
				return Get(await materialsPresenter.GetAsync(idOrName));
		}

		[NonAction]
		protected virtual IActionResult Get(MaterialView materialView)
		{
			if (materialView == null)
				return BadRequest();

			var category = categoriesCache.GetCategory(materialView.CategoryName);

			if (!materialsAuthorization.CanGet(User.Roles, category))
				return Unauthorized();

			if (materialView.IsHidden && !materialsAuthorization.CanHide(User.Roles, category))
				return Unauthorized();

			if (materialView.DeletedDate != null && !materialsAuthorization.CanRestoreAsync(User, category.Id))
				return Unauthorized();

			materialView.VisitsCount += materialsVisitsCounterCache.CountMaterial(UserOrIpKey, materialView.Id);

			return Json(materialView);
		}
		
		[HttpPost]
		public virtual async Task<IActionResult> GetMaterials(GetMaterialsRequest materialsRequest)
		{
			var section = (MaterialsServerSectionData) sectionsCache.GetSectionserverCached(materialsRequest.SectionName, User.Roles).Data;
			if(section == null)
				return BadRequest($"Can not find {materialsRequest.SectionName} section");

			if (!("," + section.CategoriesNames + ",").Contains("," + materialsRequest.CategoryName + ","))
				return BadRequest($"Can not show {materialsRequest.CategoryName} in {materialsRequest.SectionName} section");
			
			var categoryCached = categoriesCache.GetCategory(materialsRequest.CategoryName);
			if(categoryCached == null)
				return BadRequest($"Can not find {materialsRequest.CategoryName} category");
			
			string sectionTypeName = section.GetType().Name;
			string presenterName = sectionTypeName.Substring(0, materialsRequest.SectionName.Length - "ServerSectionData".Length) +
			                       "Presenter";

			MaterialsSectionsPresenterService.MaterialsSectionsPresenters.TryGetValue(presenterName,
				out Type presenterType);

			using var scope = serviceProvider.CreateScope();

			Func<IQueryable<Material>, IOrderedQueryable<Material>> sort = null;

			MaterialsSortOptionsService.MaterialsSortOptions.TryGetValue(materialsRequest.Sort,
				out sort);

			MaterialsShowOptions options = new MaterialsShowOptions()
			{
				ShowDeleted = materialsRequest.ShowDeleted,
				CategoryId =  categoryCached.Id,
				Page = materialsRequest.Page,
				Sort = sort
			};

			IMaterialsQueryPresenter materialsQueryPresenter =
				(IMaterialsQueryPresenter) scope.ServiceProvider.GetRequiredService(presenterType);

			async Task<IList<object>> LoadDataAsync()
			{
				return await materialsQueryPresenter.GetMaterialsByCategoryAsync(options);
			}

			return await CacheContentAsync(categoryCached, categoryCached.Id, LoadDataAsync, new RequestOptions()
			{
				Sort = materialsRequest.Sort,
				PageNumber = materialsRequest.Page
			});
		}

		[HttpPost]
		public virtual async Task<IActionResult> GetMaterialsFromMultiCategories(GetMaterialsRequest materialsRequest)
		{
			SectionServerCached section = sectionsCache.GetSectionserverCached(materialsRequest.SectionName, User.Roles);
			if (section == null)
				return BadRequest($"No component {materialsRequest.SectionName} found in cache");
			
			MaterialsServerSectionData sectionData = (MaterialsServerSectionData) section.Data;
			
			var materialCategoriesDic = categoriesCache.GetAllCategoriesWithChildren(sectionData.CategoriesNames);
			
			string sectionTypeName = section.GetType().Name;
			string presenterName = sectionTypeName.Substring(0, materialsRequest.SectionName.Length - "ServerSectionData".Length) +
			                       "Presenter";
			
			MaterialsSectionsPresenterService.MaterialsSectionsPresenters.TryGetValue(presenterName,
				out Type presenterType);

			IMaterialsQueryPresenter materialQueryPresenter =
				(IMaterialsQueryPresenter) serviceProvider.GetRequiredService(presenterType);

			IList<CategoryCached> categories = authorizationService.GetAllowedCategories(User.Roles,
				materialCategoriesDic.Values, operationKeysContainer.MaterialAndCommentsRead);

			if (categories.Count == 0)
				return BadRequest("No categories to show");

			IEnumerable<int> categoriesIds = categories.Select(x => x.Id).ToArray();

			MaterialsSortOptionsService.MaterialsSortOptions.TryGetValue(materialsRequest.Sort,
				out Func<IQueryable<Material>, IOrderedQueryable<Material>> sort);

			var options = new MaterialsMultiCatShowOptions
			{
				CategoriesIds = categoriesIds,
				Page = materialsRequest.Page,
				PageSize = sectionData.PageSize,
				SortType = sort
			};

			async Task<IList<object>> LoadDataAsync()
			{
				return await materialQueryPresenter.GetMaterialsFromMultiCategory(options);
			}

			return await CacheContentAsync(section, categoriesIds, LoadDataAsync, materialsRequest.Page);
		}

		[HttpPost]
		[UserSpamProtectionFilter(TimeoutSeconds = 60)]
		public virtual async Task<IActionResult> Create(MaterialRequest materialData)
		{
			var category = categoriesCache.GetCategory(materialData.CategoryName);
			if (category == null)
				return BadRequest();

			if (!materialsAuthorization.CanCreate(User.Roles, category))
				return Unauthorized();

			var now = DateTime.UtcNow;

			var material = new Material
			{
				Title = materialData.Title,
				Text = materialData.text,
				PublishDate = now,
				LastActivity = now,
				CategoryId = category.Id,
				AuthorId = User.UserId
			};

			await SetNameAsync(material, materialData.Name);

			if (category.IsMaterialsSubTitleEditable)
				material.SubTitle = materialData.SubTitle;

			if (materialData.IsHidden && materialsAuthorization.CanHide(User.Roles, category))
				material.IsHidden = true;

			if (materialData.IsHidden && materialsAuthorization.CanBlockComments(User.Roles, category))
				material.IsCommentsBlocked = true;

			if (materialsAuthorization.CanEditSettingsJson(User.Roles, category))
				material.SettingsJson = materialData.SettingsJson;

			await materialsManager.CreateAsync(material, materialData.Tags, category, User.Roles);

			contentCache.InvalidateCache(category.Id);

			return Ok();
		}

		[HttpPost]
		public virtual async Task<IActionResult> Update(MaterialRequest materialData)
		{
			if (!ModelState.IsValid)
			{
				var ers = ModelState.Values.SelectMany(v => v.Errors);
				return BadRequest(string.Join(",\n ", ers.Select(x => x.ErrorMessage)));
			}

			Material material = await materialsManager.GetAsync(materialData.Id);
			if (material == null)
				return BadRequest();

			if (!await materialsAuthorization.CanUpdateAsync(User, material))
				return Unauthorized();

			var newCategory = categoriesCache.GetCategory(materialData.CategoryName);
			if (newCategory == null)
				return BadRequest();

			material.Title = materialData.Title;
			material.Text = materialData.text;
			material.EditDate = DateTime.UtcNow;

			await SetNameAsync(material, materialData.Name);

			material.SubTitle = newCategory.IsMaterialsSubTitleEditable ? materialData.SubTitle : null;

			if (material.IsHidden != materialData.IsHidden && materialsAuthorization.CanHide(User.Roles, newCategory))
				material.IsHidden = materialData.IsHidden;

			if (material.IsCommentsBlocked != materialData.IsCommentsBlocked &&
			    materialsAuthorization.CanBlockComments(User.Roles, newCategory))
				material.IsCommentsBlocked = materialData.IsCommentsBlocked;

			if (material.CategoryId != newCategory.Id
			    && materialsAuthorization.CanMove(User, categoriesCache.GetCategory(material.CategoryId), newCategory))
			{
				contentCache.InvalidateCache(material.CategoryId);
				material.CategoryId = newCategory.Id;
			}

			material.SettingsJson = materialsAuthorization.CanEditSettingsJson(User.Roles, newCategory)
				? materialData.SettingsJson
				: null;

			await materialsManager.UpdateAsync(material, materialData.Tags, newCategory, User.Roles);

			contentCache.InvalidateCache(material.CategoryId);

			return Ok();
		}

		[NonAction]
		protected async Task SetNameAsync(Material material, string name)
		{
			if (material.Name == name)
				return;

			if (User.IsInRole(RoleNames.Admin))
			{
				if (string.IsNullOrWhiteSpace(name))
				{
					material.Name = null;
				}
				else
				{
					if (!materialsManager.IsNameValid(name))
						throw new SunErrorException(new Error("MaterialNameNotValid", "Invalid material name"));

					if (await materialsManager.IsNameInDbAsync(name))
						throw new SunErrorException(new Error("MaterialNameAlreadyUsed",
							"This material name is already used", ErrorType.Soft));

					material.Name = name;
				}
			}
		}

		[HttpPost]
		public virtual async Task<IActionResult> Delete(int id)
		{
			Material material = await materialsManager.GetAsync(id);
			if (material == null)
				return BadRequest();

			if (!await materialsAuthorization.CanDeleteAsync(User, material))
				return Unauthorized();

			contentCache.InvalidateCache(material.CategoryId);

			await materialsManager.DeleteAsync(material);

			contentCache.InvalidateCache(material.CategoryId);

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> Restore(int id)
		{
			Material material = await materialsManager.GetAsync(id);
			if (material == null)
				return BadRequest();

			if (!materialsAuthorization.CanRestoreAsync(User, material.CategoryId))
				return Unauthorized();

			await materialsManager.RestoreAsync(material);

			contentCache.InvalidateCache(material.CategoryId);

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> Down(int id, int countMove = 1)
		{
			Material material = await materialsManager.GetAsync(id);

			if (material == null)
				return BadRequest("Invalid article ID");
			if (countMove <= 0 || material.SortNumber - countMove < 1)
				return BadRequest("Incorrect count move");
			if (!await materialsAuthorization.CanUpdateAsync(User, material))
				return Forbid();

			int? categoryId = await materialsManager.GetCategoryIdAsync(id);
			await materialsManager.DownAsync(id);

			contentCache.InvalidateCache(categoryId.Value);

			return Ok();
		}

		public async Task<IActionResult> Up(int id, int countMove = 1)
		{
			if (countMove < 1)
				return BadRequest("Incorrect count move");
			Material material = await materialsManager.GetAsync(id);

			if (material == null)
				return BadRequest("Invalid article ID");

			if (!await materialsAuthorization.CanUpdateAsync(User, material))
				return Forbid();

			int? categoryId = await materialsManager.GetCategoryIdAsync(id);
			await materialsManager.UpAsync(id, countMove);

			contentCache.InvalidateCache(categoryId.Value);

			return Ok();
		}

		public class GetMaterialsRequest
		{
			public string CategoryName;
			public string SectionName;
			public int Page;
			public string Sort = null;
			public bool ShowDeleted = false;
		}

		public class MaterialRequest
		{
			public string Name { get; set; }
			public int Id { get; set; }
			public string CategoryName { get; set; }

			[Required]
			[MinLength(3)]
			[MaxLength(DbColumnSizes.Materials_Title)]
			public string Title { get; set; }

			[MaxLength(DbColumnSizes.Materials_SubTitle)]
			public string SubTitle { get; set; }

			[Required] public string text { get; set; }

			public string Tags { get; set; } = "";
			public DateTime? PublishDate { get; set; } = null;
			public int? AuthorId { get; set; } = null;

			public bool IsHidden { get; set; }
			public bool IsCommentsBlocked { get; set; }

			public string SettingsJson { get; set; }
		}
	}
}