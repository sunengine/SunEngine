using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SunEngine.Commons.Models;
using SunEngine.Commons.Utils;
using SunEngine.Options;
using SunEngine.Services;

namespace Jwt
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate next;
        private readonly JwtOptions jwtOptions;
        
        public JwtMiddleware(RequestDelegate next, IOptions<JwtOptions> jwtOptions)
        {
            this.next = next;
            this.jwtOptions = jwtOptions.Value;
        }
 
        public async Task Invoke(HttpContext context)
        {
            var name = "si";
            var cookie = context.Request.Cookies["LAT2"];

            if (cookie == null || !Validate(cookie))
                if (context.Request.Headers.ContainsKey("Authorization"))
                    context.Request.Headers.Remove("Authorization");

            await next.Invoke(context);
        }

        bool Validate(string token)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.LongJwtSecurityKey));

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Issuer,
                IssuerSigningKey = key
            };

            try
            {
                return tokenHandler.ValidateToken(token, validationParameters, out _) != null;
            }
            catch
            {
                return false;
            }
        }
    }
}