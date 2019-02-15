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
using SunEngine.Configuration.Options;
using SunEngine.Managers;
using SunEngine.Models;
using SunEngine.Security.Authorization;
using SunEngine.Stores;

namespace SunEngine.Security.Authentication
{
    public class MyJwtHandler : AuthenticationHandler<MyJwtOptions>
    {
        private readonly IRolesCache rolesCache;
        private readonly JwtOptions jwtOptions;
        private readonly JwtService jwtService;
        private readonly MyUserManager userManager;
        private readonly JwtBlackListService jwtBlackListService;

        public MyJwtHandler(
            IOptionsMonitor<MyJwtOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IRolesCache rolesCache,
            IOptions<JwtOptions> jwtOptions,
            JwtService jwtService,
            JwtBlackListService jwtBlackListService,
            MyUserManager userManager) : base(options, logger, encoder, clock)
        {
            this.rolesCache = rolesCache;
            this.jwtOptions = jwtOptions.Value;
            this.jwtService = jwtService;
            this.userManager = userManager;
            this.jwtBlackListService = jwtBlackListService;
        }


        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            AuthenticateResult ErrorAuthorization()
            {
                jwtService.MakeLogoutCookiesAndHeaders(Response);

                return AuthenticateResult.NoResult();
            }

            try
            {
                var cookie = Request.Cookies[TokenClaimNames.LongToken2CoockiName];

                if (cookie == null)
                    return AuthenticateResult.NoResult();


                JwtSecurityToken jwtLongToken2 = jwtService.ReadLongToken2(cookie);
                if (jwtLongToken2 == null)
                    return ErrorAuthorization();

                var longToken2db = jwtLongToken2.Claims.First(x => x.Type == TokenClaimNames.LongToken2Db).Value;

                MyClaimsPrincipal myClaimsPrincipal;

                if (Request.Headers.ContainsKey(Headers.LongToken1HeaderName))
                {
                    var longToken1db = Request.Headers[Headers.LongToken1HeaderName];
                    int userId = int.Parse(jwtLongToken2.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                    var longSession = new LongSession
                    {
                        UserId = userId,
                        LongToken1 = longToken1db,
                        LongToken2 = longToken2db
                    };

                    longSession = userManager.FindLongSession(longSession);

                    if (longSession == null)
                        return ErrorAuthorization();

                    myClaimsPrincipal = await jwtService.RenewSecurityTokensAsync(Response, userId, longSession);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nToken renews\n");
                    Console.ResetColor();
                }
                else
                {
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


                    var claimsPrincipal =
                        jwtService.ReadShortToken(jwtShortToken, out SecurityToken shortToken);

                    var lat2ran_1 = jwtLongToken2.Claims.FirstOrDefault(x => x.Type == TokenClaimNames.LongToken2Ran).Value;
                    var lat2ran_2 = claimsPrincipal.Claims.FirstOrDefault(x => x.Type == TokenClaimNames.LongToken2Ran).Value;

                    if (!string.Equals(lat2ran_1, lat2ran_2))
                    {
                        return ErrorAuthorization();
                    }

                    long sessionId = long.Parse(jwtLongToken2.Claims.FirstOrDefault(x => x.Type == TokenClaimNames.SessionId).Value);

                    var lat2db = jwtLongToken2.Claims.FirstOrDefault(x => x.Type == TokenClaimNames.LongToken2Db).Value;

                    myClaimsPrincipal = new MyClaimsPrincipal(claimsPrincipal, rolesCache, sessionId, lat2db);
                }

                if (jwtBlackListService.IsTokenNotInBlackList(myClaimsPrincipal.LongToken2Db))
                {
                    return ErrorAuthorization();
                }

                if (myClaimsPrincipal.Roles.ContainsKey(RoleNames.Banned))
                {
                    return ErrorAuthorization();
                }

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