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
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Managers;
using SunEngine.Core.Models;

namespace SunEngine.Core.Security
{
    /// <summary>
    /// Jwt validation handler with system of 3 tokens
    /// 1 - ShortToken (Access token), stored in client JS or localStorage Short token life approximate 5 minutes to 2 days
    /// 2 - LongToken1 (Refresh token), stored in client JS or localStorage. Long token life ~ 3 month.
    /// 3 - LongToken2 (Access + Refresh token, 2 in 1), stored in cookie Long token life ~ 3 month.
    /// LongToken2 needed to verify ShortToken and LongToken1 to protect against XSS attacks.
    /// </summary>
    public class SunJwtHandler : AuthenticationHandler<SunJwtOptions>
    {
        private readonly IRolesCache rolesCache;
        private readonly JwtOptions jwtOptions;
        private readonly JwtService jwtService;
        private readonly SunUserManager userManager;
        private readonly JwtBlackListService jwtBlackListService;

        public SunJwtHandler(
            IOptionsMonitor<SunJwtOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IRolesCache rolesCache,
            IOptions<JwtOptions> jwtOptions,
            JwtService jwtService,
            JwtBlackListService jwtBlackListService,
            SunUserManager userManager) : base(options, logger, encoder, clock)
        {
            this.rolesCache = rolesCache;
            this.jwtOptions = jwtOptions.Value;
            this.jwtService = jwtService;
            this.userManager = userManager;
            this.jwtBlackListService = jwtBlackListService;
        }


        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            async Task<AuthenticateResult> DeleteLongSessionAndLogout(long sessionId)
            {
                await userManager.DeleteLongSessionAsync(sessionId);

                jwtService.MakeLogoutCookiesAndHeaders(Response);

                return AuthenticateResult.NoResult();
            }

            AuthenticateResult Logout()
            {
                jwtService.MakeLogoutCookiesAndHeaders(Response);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\nLogout\n");
                Console.ResetColor();
                
                return AuthenticateResult.NoResult();
            }

            try
            {
                var cookie = Request.Cookies[TokenClaimNames.LongToken2CoockiName];

                if (cookie == null)
                    return AuthenticateResult.NoResult();


                JwtSecurityToken jwtLongToken2 = jwtService.ReadLong2Token(cookie);
                if (jwtLongToken2 == null)
                    return Logout();

                var longToken2db = jwtLongToken2.Claims.First(x => x.Type == TokenClaimNames.LongToken2Db).Value;

                SunClaimsPrincipal sunClaimsPrincipal;

                if (Request.Headers.ContainsKey(Headers.LongToken1HeaderName))
                {
                    string longToken1db = Request.Headers[Headers.LongToken1HeaderName];
                    int userId = int.Parse(jwtLongToken2.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                    var longSessionToFind = new LongSession
                    {
                        UserId = userId,
                        LongToken1 = longToken1db,
                        LongToken2 = longToken2db
                    };

                    var longSession = await userManager.FindLongSessionAsync(longSessionToFind);

                    if (longSession == null)
                        return Logout();

                    sunClaimsPrincipal = await jwtService.RenewSecurityTokensAsync(Response, userId, longSession);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nToken renews\n");
                    Console.ResetColor();
                }
                else
                {
                    string authorization = Request.Headers["Authorization"];

                    if (string.IsNullOrEmpty(authorization))
                        return AuthenticateResult.NoResult();

                    string jwtShortToken = null;
                    if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        jwtShortToken = authorization.Substring("Bearer ".Length).Trim();

                    if (string.IsNullOrEmpty(jwtShortToken))
                        return AuthenticateResult.NoResult();


                    var claimsPrincipal = jwtService.ReadShortToken(jwtShortToken);

                    string lat2ran_1 = jwtLongToken2.Claims.First(x => x.Type == TokenClaimNames.LongToken2Ran).Value;
                    string lat2ran_2 = claimsPrincipal.Claims.First(x => x.Type == TokenClaimNames.LongToken2Ran).Value;

                    long sessionId =
                        long.Parse(jwtLongToken2.Claims.First(x => x.Type == TokenClaimNames.SessionId).Value);

                    if (!string.Equals(lat2ran_1, lat2ran_2))
                        return await DeleteLongSessionAndLogout(sessionId);

                    string lat2db = jwtLongToken2.Claims.First(x => x.Type == TokenClaimNames.LongToken2Db).Value;

                    sunClaimsPrincipal = new SunClaimsPrincipal(claimsPrincipal, rolesCache, sessionId, lat2db);
                }

                if (jwtBlackListService.IsTokenInBlackList(sunClaimsPrincipal.LongToken2Db))
                    return await DeleteLongSessionAndLogout(sunClaimsPrincipal.SessionId);

                if (sunClaimsPrincipal.Roles.ContainsKey(RoleNames.Banned))
                    return await DeleteLongSessionAndLogout(sunClaimsPrincipal.SessionId);

                var authenticationTicket = new AuthenticationTicket(sunClaimsPrincipal, SunJwt.Scheme);
                return AuthenticateResult.Success(authenticationTicket);
            }
            catch (Exception e)
            {
                return Logout();
            }
        }
    }

    public class SunJwtOptions : AuthenticationSchemeOptions
    {
    }

    public static class SunJwt
    {
        public const string Scheme = "MyScheme";
    }
}
