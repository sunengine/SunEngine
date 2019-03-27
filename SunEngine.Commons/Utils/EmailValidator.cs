using System.ComponentModel.DataAnnotations;

namespace SunEngine.Commons.Utils
{
    public  static class EmailValidator
    {
        public static bool IsValid(string email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }
    }
}