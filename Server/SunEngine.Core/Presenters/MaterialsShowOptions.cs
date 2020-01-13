using System.Collections.Generic;

namespace SunEngine.Core.Presenters
{
	public class MaterialsShowOptions
	{
		public int CategoryId;
		public OrderType orderType;
		public int Page;
		public int PageSize;
		public bool ShowHidden;
		public bool ShowDeleted;
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