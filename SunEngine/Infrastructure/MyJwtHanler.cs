using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SunEngine.Commons.Models;
using SunEngine.EntityServices;
using SunEngine.Options;
using SunEngine.Stores;

namespace SunEngine.Infrastructure
{
    public class MyJwtHandler : AuthenticationHandler<MyJwtOptions>
    {
        private readonly IUserGroupStore userGroupStore;
        private readonly JwtOptions jwtOptions;
        private readonly AuthService authService;
        
        public MyJwtHandler(IOptionsMonitor<MyJwtOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IUserGroupStore userGroupStore, IOptions<JwtOptions> jwtOptions, AuthService authService) : base(options, logger, encoder, clock)
        {
            this.userGroupStore = userGroupStore;
            this.jwtOptions = jwtOptions.Value;
        }

        
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            AuthenticateResult ErrorAuthorization()
            {
                Response.Cookies.Append("LAT2","", 
                    new CookieOptions
                    {
                        Expires = DateTimeOffset.UtcNow.AddMonths(-1)
                    });
                
                Response.Headers.Add("TOKENSEXPIRE","");
                
                return AuthenticateResult.NoResult();
            }

            
            var cookie = Request.Cookies["LAT2"];

            if (cookie == null) 
                return ErrorAuthorization();
               

            JwtSecurityToken jwtLongToken2 = ReadLongToken2(cookie);
            if(jwtLongToken2 == null) 
                return ErrorAuthorization();
            

            if (Request.Headers.ContainsKey("LongToken1"))
            {
                var longToken1 = Request.Headers["LongToken1"];
                int userId = int.Parse(jwtLongToken2.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
                var longToken2 = jwtLongToken2.Claims.First(x => x.Type == "LAT2").Value;

                var longSession = new LongSession
                {
                    UserId = userId,
                    LongToken1 = longToken1,
                    LongToken2 = longToken2
                };
                
                longSession = authService.FindLongSession(longSession);
                
                await authService.RenewSecurityTokensAsync(Response,userId ,longSession);
            }
            
            string authorization = Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authorization))
            {
                return AuthenticateResult.NoResult();
            }

            string jwtShortToken = null;
            if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                jwtShortToken = authorization.Substring("Bearer ".Length).Trim();
            }

            if (string.IsNullOrEmpty(jwtShortToken))
            {
                return AuthenticateResult.NoResult();
            }

            ClaimsPrincipal claimsPrincipal = ReadShortToken(jwtShortToken, out SecurityToken shortToken);
            
            var LAT2R_1 = jwtLongToken2.Claims.FirstOrDefault(x => x.Type == "LAT2R").Value;
            var LAT2R_2 = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "LAT2R").Value;
            
            if(!string.Equals(LAT2R_1,LAT2R_2)) 
            {
                return AuthenticateResult.NoResult();
            }
            
            var myClaimsPrincipal = new MyClaimsPrincipal(claimsPrincipal,userGroupStore);
            var authenticationTicket = new AuthenticationTicket(myClaimsPrincipal, MyJwt.Scheme);
            return AuthenticateResult.Success(authenticationTicket);
            
            /*var mcp = new MyClaimsPrincipal(1,userGroupStore);
            var at = new AuthenticationTicket(mcp, MyJwt.Scheme);
            return AuthenticateResult.Success(at);*/
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
        
        protected ClaimsPrincipal ReadShortToken(string token, out SecurityToken securityToken)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.ShortJwtSecurityKey));

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
                return tokenHandler.ValidateToken(token, validationParameters, out securityToken);
            }
            catch
            {
                securityToken = null;
                return null;
            }
        }
    }
    
    public class MyJwtOptions : AuthenticationSchemeOptions
    {
        
    }

    public static class MyJwt
    {
        public const string Scheme = "MyScheme";
    }
}