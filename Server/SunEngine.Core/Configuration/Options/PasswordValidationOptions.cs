namespace SunEngine.Core.Configuration.Options
{
	public class PasswordValidationOptions
	{
		public bool RequireDigit { get; set; }
		public bool RequireLowercase { get; set; }
		public bool RequireNonAlphanumeric { get; set; }
		public bool RequireUppercase { get; set; }
		public int RequiredUniqueChars { get; set; }
		public int RequiredLength { get; set; }
	}
}