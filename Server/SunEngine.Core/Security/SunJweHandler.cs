using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Managers;
using SunEngine.Core.Models;


namespace SunEngine.Core.Security
{
	/// <summary>
	/// Jwe validation handler with system of 3 tokens
	/// 1 - ShortToken (Access token), stored in client JS or localStorage Short token life approximate 5 minutes to 2 days
	/// 2 - LongToken1 (Refresh token), stored in client JS or localStorage. Long token life ~ 3 month.
	/// 3 - LongToken2 (Access + Refresh token, 2 in 1), stored in cookie Long token life ~ 3 month.
	/// LongToken2 needed to verify ShortToken and LongToken1 to protect against XSS attacks.
	/// LongToken2Ran (longToken2Ran_1 and longToken2Ran_2) -
	/// random token have to be equal in ShortToken and LongToken2 to verify that no one is substituted.
	/// </summary>
	public class SunJweHandler : AuthenticationHandler<SunJweOptions>
	{
		private readonly IRolesCache rolesCache;
		private readonly JweService jweService;
		private readonly SunUserManager userManager;
		private readonly JweBlackListService jweBlackListService;

		public SunJweHandler(
			IOptionsMonitor<SunJweOptions> options,
			ILoggerFactory logger,
			UrlEncoder encoder,
			ISystemClock clock,
			IRolesCache rolesCache,
			JweService jweService,
			JweBlackListService jweBlackListService,
			SunUserManager userManager) : base(options, logger, encoder, clock)
		{
			this.rolesCache = rolesCache;
			this.jweService = jweService;
			this.userManager = userManager;
			this.jweBlackListService = jweBlackListService;
		}


		protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
		{
			if (Request.Method == HttpMethods.Get) // Allow any get requests
				return AuthenticateResult.NoResult();

			try
			{
				var cookie = Request.Cookies[TokenClaimNames.LongToken2CoockiName];

				if (cookie == null)
					return AuthenticateResult.NoResult();


				JwtSecurityToken jwtLongToken2 = jweService.ReadLong2Token(cookie);
				if (jwtLongToken2 == null)
					return Logout("No Long2 cookie token");

				var longToken2dbId = jwtLongToken2.Claims.First(x => x.Type == TokenClaimNames.LongToken2Db).Value;

				SunClaimsPrincipal sunClaimsPrincipal = null;

				string error;

				if (Request.Headers.TryGetValue(Headers.LongToken1HeaderName, out StringValues longToken1dbId))
				{
					error = await CompareLongTokens();
				}
				else
				{
					error = await CheckShortToken();
				}

				if (error != null)
					return Logout(error);

				if (jweBlackListService.IsTokenInBlackList(sunClaimsPrincipal.LongToken2Db))
				{
					await userManager.DeleteLongSessionAsync(sunClaimsPrincipal.SessionId);
					return Logout("Blacklisted");
				}

				if (sunClaimsPrincipal.Roles.ContainsKey(RoleNames.Banned))
				{
					await userManager.DeleteLongSessionAsync(sunClaimsPrincipal.SessionId);
					return Logout("Banned");
				}

				return AuthenticateResult.Success(new AuthenticationTicket(sunClaimsPrincipal, SunJwe.Scheme));


				async Task<string> CompareLongTokens()
				{
					int userId = int.Parse(jwtLongToken2.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

					var longSessionToFind = new LongSession
					{
						UserId = userId,
						LongToken1 = longToken1dbId,
						LongToken2 = longToken2dbId
					};

					var longSession = await userManager.FindLongSessionAsync(longSessionToFind);

					if (longSession == null)
						return "Session not found";

					sunClaimsPrincipal = await jweService.RenewSecurityTokensAsync(Context, userId, longSession);

					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("\nToken renews\n");
					Console.ResetColor();

					return null;
				}

				async Task<string> CheckShortToken()
				{
					string authorization = Request.Headers["Authorization"];

					if (string.IsNullOrEmpty(authorization))
						return "No Authorization header";

					string jwtShortToken = null;
					if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
						jwtShortToken = authorization.Substring("Bearer ".Length).Trim();

					if (string.IsNullOrEmpty(jwtShortToken))
						return "No Bearer in Authorization header";


					var claimsPrincipal = jweService.ReadShortToken(jwtShortToken);

					string longToken2Ran_1 =
						jwtLongToken2.Claims.First(x => x.Type == TokenClaimNames.LongToken2Ran).Value;
					string longToken2Ran_2 = claimsPrincipal.Claims.First(x => x.Type == TokenClaimNames.LongToken2Ran)
						.Value;

					long sessionId =
						long.Parse(jwtLongToken2.Claims.First(x => x.Type == TokenClaimNames.SessionId).Value);

					if (!string.Equals(longToken2Ran_1, longToken2Ran_2))
					{
						await userManager.DeleteLongSessionAsync(sessionId);
						return "lat2ran_1 != lat2ran_2";
					}

					sunClaimsPrincipal = new SunClaimsPrincipal(claimsPrincipal, rolesCache, sessionId, longToken2dbId);

					return null;
				}
			}
			catch (Exception e)
			{
				return Logout("Error " + e);
			}


			AuthenticateResult Logout(string msg)
			{
				jweService.MakeLogoutCookiesAndHeaders(Response);

				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine($"\nLogout: {msg}\n");
				Console.ResetColor();

				return AuthenticateResult.NoResult();
			}
		}
	}

	public class SunJweOptions : AuthenticationSchemeOptions
	{
	}

	public static class SunJwe
	{
		public const string Scheme = "SunScheme";
	}
}