using SunEngine.Commons.Models.Authorization;

namespace SunEngine.Commons.Cache.CacheModels
{
    public class OperationKeyCached
    {
        public int OperationKeyId { get;  }
        public string Name { get;  }
        
        public OperationKeyCached(OperationKey operationKey)
        {
            OperationKeyId = operationKey.OperationKeyId;
            Name = operationKey.Name;
        }
    }
}