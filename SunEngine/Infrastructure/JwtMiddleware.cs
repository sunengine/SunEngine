using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SunEngine.Commons.Models;
using SunEngine.Commons.Utils;
using SunEngine.EntityServices;
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

        public async Task InvokeAsync(HttpContext context, AuthService authService)
        {
            var cookie = context.Request.Cookies["LAT2"];

            if (cookie == null)
                if (context.Request.Headers.ContainsKey("Authorization"))
                    context.Request.Headers.Remove("Authorization");

            var jwtLongToken2 = ReadLongToken2(cookie);

            if (context.Request.Headers.ContainsKey("LongToken1"))
            {
                var longToken1 = context.Request.Headers["LongToken1"];
                int userId = int.Parse(jwtLongToken2.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
                var longToken2 = jwtLongToken2.Claims.First(x => x.Type == "LAT2").Value;

                var longSession = new LongSession
                {
                    UserId = userId,
                    LongToken1 = longToken1,
                    LongToken2 = longToken2
                };
                
                longSession = authService.FindLongSession(longSession);
                
                await authService.RenewSecurityTokensAsync(context.Response,userId ,longSession);
            }

            await next.Invoke(context);
        }

        protected JwtSecurityToken ReadLongToken2(string token)
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
                var principal =
                    tokenHandler.ValidateToken(token, validationParameters, out SecurityToken securityToken);
                if (principal != null)
                    return (JwtSecurityToken) securityToken;
                else
                    return null;
            }
            catch
            {
                return null;
            }
        }
    }
}