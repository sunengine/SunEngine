using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
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
    public class SunJweHandler : AuthenticationHandler<SunJwtOptions>
    {
        private readonly IRolesCache rolesCache;
        private readonly JweOptions jweOptions;
        private readonly JweService jweService;
        private readonly SunUserManager userManager;
        private readonly JweBlackListService jweBlackListService;

        public SunJweHandler(
            IOptionsMonitor<SunJwtOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IRolesCache rolesCache,
            IOptions<JweOptions> jweOptions,
            JweService jweService,
            JweBlackListService jweBlackListService,
            SunUserManager userManager) : base(options, logger, encoder, clock)
        {
            this.rolesCache = rolesCache;
            this.jweOptions = jweOptions.Value;
            this.jweService = jweService;
            this.userManager = userManager;
            this.jweBlackListService = jweBlackListService;
        }


        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            async Task<AuthenticateResult> DeleteLongSessionAndLogout(long sessionId)
            {
                await userManager.DeleteLongSessionAsync(sessionId);

                jweService.MakeLogoutCookiesAndHeaders(Response);

                return AuthenticateResult.NoResult();
            }

            AuthenticateResult Logout()
            {
                jweService.MakeLogoutCookiesAndHeaders(Response);

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


                JwtSecurityToken jwtLongToken2 = jweService.ReadLong2Token(cookie);
                if (jwtLongToken2 == null)
                    return Logout();

                var longToken2db = jwtLongToken2.Claims.First(x => x.Type == TokenClaimNames.LongToken2Db).Value;

                SunClaimsPrincipal sunClaimsPrincipal;

                if (Request.Headers.TryGetValue(Headers.LongToken1HeaderName, out StringValues longToken1db))
                {
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

                    sunClaimsPrincipal = await jweService.RenewSecurityTokensAsync(Context, userId, longSession);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nToken renews\n");
                    Console.ResetColor();
                }
                else
                {
                    string authorization = Request.Headers["Authorization"];

                    if (string.IsNullOrEmpty(authorization))
                        return Logout();

                    string jwtShortToken = null;
                    if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                        jwtShortToken = authorization.Substring("Bearer ".Length).Trim();

                    if (string.IsNullOrEmpty(jwtShortToken))
                        return Logout();


                    var claimsPrincipal = jweService.ReadShortToken(jwtShortToken);

                    string lat2ran_1 = jwtLongToken2.Claims.First(x => x.Type == TokenClaimNames.LongToken2Ran).Value;
                    string lat2ran_2 = claimsPrincipal.Claims.First(x => x.Type == TokenClaimNames.LongToken2Ran).Value;

                    long sessionId =
                        long.Parse(jwtLongToken2.Claims.First(x => x.Type == TokenClaimNames.SessionId).Value);

                    if (!string.Equals(lat2ran_1, lat2ran_2))
                        return await DeleteLongSessionAndLogout(sessionId);

                    string lat2db = jwtLongToken2.Claims.First(x => x.Type == TokenClaimNames.LongToken2Db).Value;

                    sunClaimsPrincipal = new SunClaimsPrincipal(claimsPrincipal, rolesCache, sessionId, lat2db);
                }

                if (jweBlackListService.IsTokenInBlackList(sunClaimsPrincipal.LongToken2Db))
                    return await DeleteLongSessionAndLogout(sunClaimsPrincipal.SessionId);

                if (sunClaimsPrincipal.Roles.ContainsKey(RoleNames.Banned))
                    return await DeleteLongSessionAndLogout(sunClaimsPrincipal.SessionId);

                var authenticationTicket = new AuthenticationTicket(sunClaimsPrincipal, SunJwt.Scheme);
                return AuthenticateResult.Success(authenticationTicket);
            }
            catch (Exception)
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
