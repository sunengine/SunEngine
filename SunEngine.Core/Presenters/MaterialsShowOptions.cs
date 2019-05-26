using System.Collections.Generic;

namespace SunEngine.Core.Presenters
{
    public class MaterialsShowOptions
    {
        public int CategoryId;
        public IEnumerable<int> CategoriesIds;
        public ArticlesOrderType ArticlesOrderType;
        public bool ShowHidden;
        public bool ShowDeleted;
        public int Page;
        public int PageSize;
    }
}