using System.Collections.Generic;
using System.Linq;
using SunEngine.Models;

namespace SunEngine.Stores
{
    public static class CategoryExtensions
    {
        public static IDictionary<string, Category> GetAllSubcategories(this Category category)
        {
            var rez = category.SubCategories.ToDictionary(x => x.Name, x => x);
            foreach (var subCategory in category.SubCategories)
            {
                var sub = subCategory.GetAllSubcategories();
                foreach (var (key, value) in sub)
                {
                    rez.Add(key,value);
                }
            }

            return rez;
        }
    }
}