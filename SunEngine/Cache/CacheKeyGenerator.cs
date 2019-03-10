using System.Collections.Generic;

namespace SunEngine.Stores
{
    public class CacheKeyGenerator
    {
        public string ContentGenerateKey(string controllerName, string actionName, IEnumerable<int> indexes, int page) 
        {
            return indexes == null 
                ? string.Empty 
                : $"{controllerName}-{actionName}-{page}:,{string.Join(',', indexes)},";
        }

        public string ContentGenerateKey(string controllerName, string actionName, int index, int page)
        {
            return $"{controllerName}-{actionName}-{page}:,{index},";
        }
    }
}