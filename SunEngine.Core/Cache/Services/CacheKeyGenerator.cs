using System.Collections.Generic;
using System.Linq;

namespace SunEngine.Core.Cache.Services
{
    public class CacheKeyGenerator
    {
        public string ContentGenerateKey(string controllerName, string actionName, IEnumerable<int> ids, int? page)
        {
            return ids == null || !ids.Any()
                ? null
                : $"{controllerName}-{actionName}-{page}:,{string.Join(',', ids.OrderBy(x => x).Distinct())},";
        }

        public string ContentGenerateKey(string controllerName, string actionName, int? page, int id)
        {
            return $"{controllerName}-{actionName}-{page}:,{id},";
        }

        public string ContentGenerateKey(string componentName, int? page)
        {
            return $"{componentName}-{page}";
        }

        public string ContentGenerateKey(string componentName, IEnumerable<int> ids, int? page)
        {
            return $"{componentName}-{page}:,{string.Join(',', ids.OrderBy(x => x).Distinct())},";
        }
    }
}
