using SunEngine.Models.Authorization;

namespace SunEngine.Stores.Models
{
    public class OperationKeyStored
    {
        public int OperationKeyId { get;  }
        public string Name { get;  }
        
        public OperationKeyStored(OperationKey operationKey)
        {
            OperationKeyId = operationKey.OperationKeyId;
            Name = operationKey.Name;
        }
    }
}