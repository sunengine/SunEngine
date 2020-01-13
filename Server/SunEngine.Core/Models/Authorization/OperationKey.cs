namespace SunEngine.Core.Models.Authorization
{
	/// <summary>
	/// Name of operation to allow or disallow access for Role
	/// </summary>
	public class OperationKey
	{
		public int OperationKeyId { get; set; }
		public string Name { get; set; }
	}
}