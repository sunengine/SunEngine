using System;
using System.Collections.Generic;
using System.Linq;
using SunEngine.Core.Models.Materials;

namespace SunEngine.Materials
{
	public class MaterialsShowOptions
	{
		public int CategoryId;
		public IEnumerable<int> CategoriesIds;
		public int Page;
		public int PageSize;
		public int PreviewSize;
		public bool ShowHidden;
		public bool ShowDeleted;
		public Func<IQueryable<Material>, IOrderedQueryable<Material>> Sort;
	}
}