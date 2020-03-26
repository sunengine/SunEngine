using System;
using System.Collections.Generic;
using System.Linq;
using SunEngine.Core.Models.Materials;

namespace SunEngine.Core.Presenters
{
	interface IMaterialsSort
	{
		IQueryable<Material> SortAsc(IQueryable<Material> query);
		IQueryable<Material> SortDesc(IQueryable<Material> query);
	}
	
	public class MaterialsShowOptions
	{
		public int CategoryId;
		public OrderType orderType;
		public int Page;
		public int PageSize;
		public bool ShowHidden;
		public bool ShowDeleted;
		public Func<IQueryable<Material>,IQueryable<Material>> Sort;
	}

	public class MaterialsMultiCatShowOptions
	{
		public IEnumerable<int> CategoriesIds;
		public OrderType orderType;
		public int Page;
		public int PageSize;
		public int PreviewSize;
	}
}