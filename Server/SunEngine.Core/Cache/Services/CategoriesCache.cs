using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using LinqToDB.Common;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;

namespace SunEngine.Core.Cache.Services
{
	/// <summary>
	/// Store categories in cache to fast access for singleton service
	/// </summary>
	public interface ICategoriesCache
	{
		CategoryCached GetCategory(int id);
		CategoryCached GetCategory(string name);
		CategoryCached RootCategory { get; }

		/// <summary>
		/// Get full list categories and all children categories 
		/// </summary>
		/// <param name="categoriesList">Comma separated list on categories names</param>
		IDictionary<string, CategoryCached> GetAllCategoriesWithChildren(string categoriesList);

		void Initialize();
	}

	/// <summary>
	/// Store categories in cache to fast access for singleton service
	/// </summary>
	public class CategoriesCache : ICategoriesCache
	{
		protected readonly IDataBaseFactory dataBaseFactory;
		public IReadOnlyDictionary<string, CategoryCached> AllCategoriesByName { get; protected set; }
		public IReadOnlyDictionary<int, CategoryCached> AllCategoriesById { get; protected set; }
		public CategoryCached RootCategory { get; protected set; }

		public CategoriesCache(IDataBaseFactory dataBaseFactory)
		{
			this.dataBaseFactory = dataBaseFactory;
			Initialize();
		}

		public CategoryCached GetCategory(int id)
		{
			return AllCategoriesById[id];
		}

		public CategoryCached GetCategory(string name)
		{
			return AllCategoriesByName[name];
		}

		public IDictionary<string, CategoryCached> GetAllCategoriesWithChildren(string categoriesList)
		{
			Dictionary<string, CategoryCached> materialsCategoriesDic = new Dictionary<string, CategoryCached>();

			if (categoriesList.IsNullOrEmpty())
				return materialsCategoriesDic;

			var categoriesNames = categoriesList.Split(',').Select(x => x.Trim());
			foreach (var name in categoriesNames)
			{
				CategoryCached category = GetCategory(name);
				var allSub = category.AllSubCategories.ToDictionary(x => x.Name, x => x);
				allSub.Add(category.Name, category);

				foreach (var (key, value) in allSub)
					if (!materialsCategoriesDic.ContainsKey(key))
						materialsCategoriesDic.Add(key, value);
			}

			return materialsCategoriesDic;
		}

		public void Initialize()
		{
			using var db = dataBaseFactory.CreateDb();
			var categories = db.Categories.Where(x => x.DeletedDate == null).Select(x => new CategoryCached(x))
				.ToDictionary(x => x.Id);

			foreach (var category in categories.Values)
				category.Init1_ParentAndSub(categories);

			RootCategory = categories.Values.FirstOrDefault(x => x.Name == Category.RootCategoryName);
			if (RootCategory == null)
				throw new Exception($"Can not find category '{Category.RootCategoryName}' in data base.");

			var categoriesList = RootCategory.Init2_AllSub();
			categoriesList.Insert(0, RootCategory);

			RootCategory.Init3_UrlPaths();

			RootCategory.Init4_InitSectionsRoots();

			foreach (var category in categoriesList)
				category.Init5_SetListsAndFreeze();

			AllCategoriesByName =
				categoriesList.ToImmutableDictionary(x => x.NameNormalized, StringComparer.OrdinalIgnoreCase);

			AllCategoriesById = AllCategoriesByName.ToImmutableDictionary(x => x.Value.Id, x => x.Value);
		}
	}
}