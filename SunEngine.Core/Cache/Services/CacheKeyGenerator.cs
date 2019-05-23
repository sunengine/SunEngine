using System.Linq;

namespace SunEngine.Core.Cache.Services
{
    public class CacheKeyGenerator
    {
        public string ContentGenerateKey(string controllerName, string actionName, int page, params int[] ids)
        {
            return ids == null || !ids.Any()
                ? null : $"{controllerName}-{actionName}-{page}:,{string.Join(',', ids)},";
        }
    }
}