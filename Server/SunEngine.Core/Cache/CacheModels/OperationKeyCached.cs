using SunEngine.Core.Models.Authorization;

namespace SunEngine.Core.Cache.CacheModels
{
	/// <summary>
	/// Cache of OperationKey for RolesCache
	/// </summary>
	public class OperationKeyCached
	{
		public int OperationKeyId { get; }
		public string Name { get; }

		public OperationKeyCached(OperationKey operationKey)
		{
			OperationKeyId = operationKey.OperationKeyId;
			Name = operationKey.Name;
		}
	}
}