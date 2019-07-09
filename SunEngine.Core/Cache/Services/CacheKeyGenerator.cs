using System.Collections.Generic;
using System.Linq;

namespace SunEngine.Core.Cache.Services
{
    public class CacheKeyGenerator
    {
        public string ContentGenerateKey(string controllerName, string actionName, int? page, IEnumerable<int> ids)
        {
            return ids == null || !ids.Any()
                ? null : $"{controllerName}-{actionName}-{page}:,{string.Join(',', ids)},";
        }

        public string ContentGenerateKey(string controllerName, string actionName, int? page, int id)
        {
            return $"{controllerName}-{actionName}-{page}:,{id},";
        }
    }
}