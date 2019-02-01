using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SunEngine.Commons.Models;
using SunEngine.Commons.Utils;
using SunEngine.Services;

namespace Jwt
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate next;

        public JwtMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
 
        public async Task Invoke(HttpContext context)
        {
            var name = "si";
            var cookie = context.Request.Cookies[name];

            if (cookie != null)
                if (!context.Request.Headers.ContainsKey("Authorization"))
                    context.Request.Headers.Append("Authorization", "Bearer " + cookie);

            await next.Invoke(context);
        }
    }

    public static class ResponseExtensions
    {
        public static void AddSequrityTokens(this HttpResponse response, User user)
        {
            if(!user.CheckTokens())
                user.RenewTokens();
            
            response.Headers.Add("LAT1",user.AuthLongToken1);
            
           CryptService cryptService = new CryptService();
               
            
            /*response.Cookies.Append(
                "LAT2",
                token,
                new CookieOptions
                {
                    Path = "/", 
                    HttpOnly = true, 
                    IsEssential = true,
                    Expires = user.AuthLongTokenExpiration.Value
                }
            );*/
        }
    }
}