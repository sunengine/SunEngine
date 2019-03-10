using System.Collections.Generic;

namespace SunEngine.Stores
{
    public class CacheKeyGenerator
    {
        public string ContentGenerateKey(string controllerName, string actionName, IEnumerable<int> indexes) 
        {
            return indexes == null ? string.Empty : $"{controllerName}_{actionName}:,{string.Join(',', indexes)},";
        }

        public string ContentGenerateKey(string controllerName, string actionName, int index)
        {
            return string.Format("{0}_{1}:,{2},", controllerName, actionName, index);
        }
    }
}