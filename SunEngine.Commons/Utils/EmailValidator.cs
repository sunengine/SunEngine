using System.ComponentModel.DataAnnotations;

namespace SunEngine.Commons.Utils
{
    public  static class EmailValidator
    {
        public static bool IsValidEmail(string email)
        {
            EmailAddressAttribute emailValidator = new EmailAddressAttribute();
            return emailValidator.IsValid(email);
        }
    }
}