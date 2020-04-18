using System;
using System.Collections.Generic;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Materials.Presenters;

namespace SunEngine.Materials.Services
{
	public static class MaterialsPresenterTypes
	{
		public static Dictionary<string, Type> Types => new Dictionary<string, Type>()
		{
			[nameof(MaterialsPresenter)] = typeof(MaterialsPresenter),
			[nameof(ForumPresenter)] = typeof(ForumPresenter),
			[nameof(ArticlesPresenter)] = typeof(ArticlesPresenter),
			[nameof(BlogPresenter)] = typeof(BlogPresenter)
		};

		public static Type GetBySection(SectionServerCached section)
		{
			string sectionTypeName = section.Data.GetType().Name;
			string presenterName =
				sectionTypeName.Substring(0, sectionTypeName.Length - "ServerSection".Length) +
				"Presenter";
			return Types[presenterName];
		}
	}
}