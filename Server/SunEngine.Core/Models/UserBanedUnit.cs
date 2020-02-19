namespace SunEngine.Core.Models
{
	/// <summary>
	/// Many to many relationship on users UserId (field) baned UserBannedId (filed)
	/// </summary>
	public class UserBanedUnit
	{
		public int UserId { get; set; }

		/// <summary>
		/// This user ban UserBaned (filed)
		/// </summary>
		public User User { get; set; }

		public int UserBanedId { get; set; }

		/// <summary>
		/// This user baned by User (field)
		/// </summary>
		public User UserBaned { get; set; }
	}
}