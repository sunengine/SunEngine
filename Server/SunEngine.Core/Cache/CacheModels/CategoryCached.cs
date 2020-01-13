using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.Json;
using Flurl;
using SunEngine.Core.Models;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Cache.CacheModels
{
	/// <summary>
	/// Cache of category for CategoriesCache
	/// </summary>
	public class CategoryCached
	{
		public int Id { get; }

		public string Name { get; }

		public string Token { get; }

		public bool AppendTokenToSubCatsPath { get; }

		public bool ShowInBreadcrumbs { get; }

		public string UrlPath { get; private set; } = "";

		public string NameNormalized { get; }

		public string Title { get; }

		public string SubTitle { get; }

		public string Icon { get; }

		public bool IsMaterialsContainer { get; }

		public string Header { get; }

		public CategoryCached SectionRoot { get; private set; }

		public bool IsMaterialsNameEditable { get; }

		public bool IsMaterialsSubTitleEditable { get; }

		public JsonElement? SettingsJson { get; }

		public int? ParentId { get; }
		public CategoryCached Parent { get; private set; }

		public IImmutableList<CategoryCached> SubCategories { get; private set; }

		public IImmutableList<CategoryCached> AllSubCategories { get; private set; }

		protected List<CategoryCached> _subCategories { get; private set; }

		protected List<CategoryCached> _allSubCategories { get; private set; }

		public int SortNumber { get; }

		public string LayoutName { get; }

		public bool IsHidden { get; }

		public bool IsCacheContent { get; }


		protected bool initialized = false;

		public CategoryCached(Category category)
		{
			Id = category.Id;
			Name = category.Name;
			NameNormalized = category.NameNormalized;
			Token = category.Token ?? category.Name.ToLower();
			AppendTokenToSubCatsPath = category.AppendTokenToSubCatsPath;
			ShowInBreadcrumbs = category.ShowInBreadcrumbs;
			Title = category.Title;
			IsMaterialsContainer = category.IsMaterialsContainer;
			SubTitle = category.SubTitle;
			Icon = category.Icon;
			Header = category.Header;
			IsMaterialsNameEditable = category.IsMaterialsNameEditable;
			IsMaterialsSubTitleEditable = category.IsMaterialsSubTitleEditable;
			SettingsJson = SunJson.MakeJElement(category.SettingsJson);
			ParentId = category.ParentId;
			SortNumber = category.SortNumber;
			LayoutName = category.LayoutName;
			IsHidden = category.IsHidden;
			IsCacheContent = category.IsCacheContent;

			_subCategories = new List<CategoryCached>();
			_allSubCategories = new List<CategoryCached>();
		}

		public void Init1_ParentAndSub(Dictionary<int, CategoryCached> allCategories)
		{
			if (initialized)
				return;

			if (ParentId.HasValue && allCategories.ContainsKey(ParentId.Value))
			{
				Parent = allCategories[ParentId.Value];
				Parent._subCategories.Add(this);
			}
		}

		public List<CategoryCached> Init2_AllSub()
		{
			if (initialized)
				return null;
			foreach (var category in _subCategories)
			{
				var list = category.Init2_AllSub();

				foreach (var sub in list)
					_allSubCategories.Add(sub);
			}

			var rez = _allSubCategories.ToList();
			rez.Add(this);
			return rez;
		}

		public void Init3_UrlPaths()
		{
			if (AppendTokenToSubCatsPath)
				foreach (var subCategory in _subCategories)
					subCategory.UrlPath = subCategory.UrlPath.AppendPathSegment(Token);

			foreach (var subCategory in _subCategories)
				subCategory.Init3_UrlPaths();

			UrlPath = UrlPath.AppendPathSegment(Token);
		}


		public void Init4_InitSectionsRoots(CategoryCached sectionRoot = null)
		{
			if (initialized)
				return;

			if (LayoutName != null)
				sectionRoot = this;

			SectionRoot = sectionRoot;

			foreach (var category in _subCategories)
				category.Init4_InitSectionsRoots(sectionRoot);
		}


		public void Init5_SetListsAndFreeze()
		{
			if (initialized)
				return;

			AllSubCategories = _allSubCategories.ToImmutableList();
			SubCategories = _subCategories.ToImmutableList();
			_allSubCategories = null;
			_subCategories = null;
			initialized = true;
		}
	}
}