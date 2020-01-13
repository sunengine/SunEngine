namespace SunEngine.Core.Models.Authorization
{
	/// <summary>
	/// Allow access or disallow to OperationKey for CategoryAccess  
	/// </summary>
	public class CategoryOperationAccess
	{
		public int CategoryAccessId { get; set; }
		public CategoryAccess CategoryAccess { get; set; }

		public int OperationKeyId { get; set; }
		public OperationKey OperationKey { get; set; }

		public bool Access { get; set; }
	}
}