using System.ComponentModel.DataAnnotations;

namespace SunEngine.Core.Utils
{
	public static class EmailValidator
	{
		public static bool IsValid(string email)
		{
			return new EmailAddressAttribute().IsValid(email);
		}
	}
}