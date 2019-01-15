using SunEngine.Commons.Models.UserGroups;

namespace SunEngine.Commons.StoreModels
{
    public class OperationKey
    {
        public int OperationKeyId { get;  }
        public string Name { get;  }
        
        public OperationKey(OperationKeyDB operationKeyDb)
        {
            OperationKeyId = operationKeyDb.OperationKeyId;
            Name = operationKeyDb.Name;
        }
    }
}