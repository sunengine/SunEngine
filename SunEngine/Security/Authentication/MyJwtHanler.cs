using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.EntityServices;
using SunEngine.Options;
using SunEngine.Stores;

namespace SunEngine.Authentication
{
    public class MyJwtHandler : AuthenticationHandler<MyJwtOptions>
    {
        private readonly IUserGroupStore userGroupStore;
        private readonly JwtOptions jwtOptions;
        private readonly AuthService authService;
        private readonly MyUserManager userManager;

        public MyJwtHandler(IOptionsMonitor<MyJwtOptions> options, ILoggerFactory logger, UrlEncoder encoder,
            ISystemClock clock, IUserGroupStore userGroupStore, IOptions<JwtOptions> jwtOptions,
            AuthService authService,
            MyUserManager userManager) : base(options, logger, encoder, clock)
        {
            this.userGroupStore = userGroupStore;
            this.jwtOptions = jwtOptions.Value;
            this.authService = authService;
            this.userManager = userManager;
        }


        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            AuthenticateResult ErrorAuthorization()
            {
                authService.MakeLogoutCookiesAndHeaders(Response);

                return AuthenticateResult.NoResult();
            }
            
            try
            {
                var cookie = Request.Cookies["LAT2"];

                if (cookie == null)
                    return  AuthenticateResult.NoResult();


                JwtSecurityToken jwtLongToken2 = authService.ReadLongToken2(cookie);
                if (jwtLongToken2 == null)
                    return ErrorAuthorization();

                var longToken2 = jwtLongToken2.Claims.First(x => x.Type == "LAT2").Value;

                if (Request.Headers.ContainsKey("LongToken1"))
                {
                    var longToken1 = Request.Headers["LongToken1"];
                    int userId = int.Parse(jwtLongToken2.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                    var longSession = new LongSession
                    {
                        UserId = userId,
                        LongToken1 = longToken1,
                        LongToken2 = longToken2
                    };

                    longSession = userManager.FindLongSession(longSession);

                    if (longSession == null)
                        return ErrorAuthorization();

                    await authService.RenewSecurityTokensAsync(Response, userId, longSession);

                    Console.WriteLine("Token Renews");
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


                ClaimsPrincipal claimsPrincipal =
                    authService.ReadShortToken(jwtShortToken, out SecurityToken shortToken);

                var LAT2R_1 = jwtLongToken2.Claims.FirstOrDefault(x => x.Type == "LAT2R").Value;
                var LAT2R_2 = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == "LAT2R").Value;

                if (!string.Equals(LAT2R_1, LAT2R_2))
                {
                    return ErrorAuthorization();
                }

                long sessionId = long.Parse(jwtLongToken2.Claims.FirstOrDefault(x => x.Type == "ID").Value);

                var myClaimsPrincipal = new MyClaimsPrincipal(claimsPrincipal, userGroupStore, sessionId);
                var authenticationTicket = new AuthenticationTicket(myClaimsPrincipal, MyJwt.Scheme);
                return AuthenticateResult.Success(authenticationTicket);
            }
            catch (Exception e)
            {
                return ErrorAuthorization();
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