using System;
using SunEngine.Commons.Models;

namespace SunEngine.Commons.Utils
{
    public static class UserExtensions
    {
        public static void RenewTokens(this User user)
        {
            user.AuthLongToken1 = CryptoRandomizer.GetRandomString(16);
            user.AuthLongToken2 = CryptoRandomizer.GetRandomString(16);
            user.AuthLongTokenExpiration = DateTime.UtcNow.AddDays(90);
        }
        
        public static bool CheckTokens(this User user)
        {
            if (!user.AuthLongTokenExpiration.HasValue)
                return false;
            
            return user.AuthLongTokenExpiration.Value >= DateTime.UtcNow;
        }
    }
}