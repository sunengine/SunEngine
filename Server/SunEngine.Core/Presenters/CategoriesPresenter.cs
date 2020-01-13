using System.Collections.Generic;
using System.Linq;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Security;

namespace SunEngine.Core.Presenters
{
	public interface ICategoriesPresenter
	{
		CategoryInfoWithAccesses GetRootCategoryInfoWithAccesses(
			IReadOnlyDictionary<string, RoleCached> roles);
	}

	public class CategoriesPresenter : ICategoriesPresenter
	{
		protected readonly OperationKeysContainer OperationKeys;

		protected readonly IAuthorizationService authorizationService;
		protected readonly ICategoriesCache categoriesCache;
		protected readonly IRolesCache rolesCache;


		public CategoriesPresenter(
			IRolesCache rolesCache,
			ICategoriesCache categoriesCache,
			IAuthorizationService authorizationService,
			OperationKeysContainer operationKeysContainer)
		{
			OperationKeys = operationKeysContainer;

			this.authorizationService = authorizationService;
			this.categoriesCache = categoriesCache;
			this.rolesCache = rolesCache;
		}

		public virtual CategoryInfoWithAccesses GetRootCategoryInfoWithAccesses(
			IReadOnlyDictionary<string, RoleCached> roles)
		{
			return GetCategoryInfoWithAccesses(categoriesCache.RootCategory, roles);
		}

		public virtual CategoryInfoWithAccesses GetCategoryInfoWithAccesses(
			CategoryCached category,
			IReadOnlyDictionary<string, RoleCached> roles)
		{
			if (!authorizationService.HasAccess(roles, category, OperationKeys.MaterialAndCommentsRead) &&
			    category.Id != categoriesCache.RootCategory.Id)
				return null;

			CategoryInfoWithAccesses categoryInfo = new CategoryInfoWithAccesses
			{
				Id = category.Id,
				Name = category.Name,
				UrlPath = category.UrlPath,
				ShowInBreadcrumbs = category.ShowInBreadcrumbs,
				Title = category.Title,
				SubTitle = category.SubTitle,
				Icon = category.Icon,
				Header = category.Header,
				IsMaterialsContainer = category.IsMaterialsContainer,
				SortNumber = category.SortNumber,
				LayoutName = category.LayoutName,
				IsMaterialsSubTitleEditable = category.IsMaterialsSubTitleEditable,
				IsMaterialsNameEditable = category.IsMaterialsNameEditable,
				SettingsJson = category.SettingsJson,
				IsHidden = category.IsHidden,

				CategoryPersonalAccess = DetectPersonalAccesses(category, roles)
			};


			if (category.SubCategories == null)
				return categoryInfo;

			IEnumerable<CategoryCached> where;
			if (roles.Any(x => x.Value.Name == RoleNames.Admin)) // Admin can see all categories, including hidden
				where = category.SubCategories;
			else
				where = category.SubCategories.Where(x => !x.IsHidden);

			if (!where.Any())
				return categoryInfo;

			categoryInfo.SubCategories = new List<CategoryInfoWithAccesses>(category.SubCategories.Count);

			foreach (var child in where.OrderBy(x => x.SortNumber))
			{
				var childInfo = GetCategoryInfoWithAccesses(child, roles);
				if (childInfo == null)
					continue;

				categoryInfo.SubCategories.Add(childInfo);
			}

			return categoryInfo;
		}

		protected Dictionary<string, bool> DetectPersonalAccesses(
			CategoryCached category,
			IReadOnlyDictionary<string, RoleCached> roles)
		{
			Dictionary<string, bool> dict = new Dictionary<string, bool>(rolesCache.AllOperationKeys.Count);

			foreach (var operationKey in rolesCache.AllOperationKeys)
			{
				bool allow = authorizationService.HasAccess(roles, category, operationKey.OperationKeyId);

				if (allow)
					dict[operationKey.Name] = true;
			}

			return dict;
		}
	}


	public class CategoryInfoWithAccesses
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string UrlPath { get; set; }
		public bool ShowInBreadcrumbs { get; set; }
		public string Title { get; set; }
		public string SubTitle { get; set; }
		public string Icon { get; set; }
		public string Header { get; set; }
		public int SortNumber { get; set; }
		public string LayoutName { get; set; }
		public bool IsMaterialsSubTitleEditable { get; set; }
		public bool IsMaterialsNameEditable { get; set; }
		public object SettingsJson { get; set; }
		public bool IsMaterialsContainer { get; set; }
		public bool IsHidden { get; set; }
		public Dictionary<string, bool> CategoryPersonalAccess { get; set; }
		public List<CategoryInfoWithAccesses> SubCategories { get; set; }
	}
}