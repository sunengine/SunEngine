using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Controllers;
using SunEngine.Core.DataBase;
using SunEngine.Core.Managers;
using SunEngine.Core.Models;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;
using UAParser;


namespace SunEngine.Core.Security
{
    /// <summary>
    /// Functions for generating, renewing and reading Jwt tokens.
    /// </summary>
    public class JweService : DbService
    {
        private readonly SunUserManager userManager;
        private readonly JweOptions jweOptions;
        private readonly ILogger logger;
        private readonly ICryptService cryptService;
        private readonly IRolesCache rolesCache;

        public JweService(
            DataBaseConnection db,
            SunUserManager userManager,
            IRolesCache rolesCache,
            ICryptService cryptService,
            IOptions<JweOptions> jwtOptions,
            ILoggerFactory loggerFactory) : base(db)
        {
            this.userManager = userManager;
            this.cryptService = cryptService;
            this.jweOptions = jwtOptions.Value;
            logger = loggerFactory.CreateLogger<AccountController>();
            this.rolesCache = rolesCache;
        }

        private readonly JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };


        public async Task<SunClaimsPrincipal> RenewSecurityTokensAsync(
            HttpContext httpContext, int userId,
            LongSession longSession = null)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return await RenewSecurityTokensAsync(httpContext, user, longSession);
        }

        public Task<SunClaimsPrincipal> RenewSecurityTokensAsync(
            HttpContext httpContext, User user,
            long sessionId)
        {
            var longSession = db.LongSessions.FirstOrDefault(x => x.Id == sessionId);
            return RenewSecurityTokensAsync(httpContext, user, longSession);
        }

        public async Task<SunClaimsPrincipal> RenewSecurityTokensAsync(
            HttpContext httpContext, User user,
            LongSession longSession = null)
        {
            void GenerateTokens(LongSession longSession1)
            {
                longSession1.LongToken1 = CryptoRandomizer.GetRandomString(LongSession.LongToken1Length);
                longSession1.LongToken2 = CryptoRandomizer.GetRandomString(LongSession.LongToken2Length);
                longSession1.ExpirationDate = DateTime.UtcNow.AddDays(jweOptions.LongTokenLiveTimeDays);
                httpContext.Request.Headers.TryGetValue("User-Agent", out StringValues userAgent);
                longSession1.DeviceInfo = Parser.GetDefault()?.Parse(userAgent.ToString() ?? "")?.ToString() ?? "";
                longSession1.UpdateDate = DateTime.UtcNow;
            }

            if (longSession == null)
            {
                longSession = new LongSession
                {
                    UserId = user.Id
                };

                GenerateTokens(longSession);

                longSession.Id = await db.InsertWithInt64IdentityAsync(longSession);
            }
            else
            {
                GenerateTokens(longSession);
                
                await db.UpdateAsync(longSession);
            }


            var lat2Token = CreateLong2Token(longSession, out string lat2r);

            httpContext.Response.Cookies.Append(
                TokenClaimNames.LongToken2CoockiName,
                lat2Token,
                new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,
                    IsEssential = true,
                    Expires = longSession.ExpirationDate
                }
            );

            TokenAndClaimsPrincipal tokenAndClaimsPrincipal =
                await CreateShortTokenAsync(user, lat2r, longSession.LongToken2, longSession.Id);

            string json = JsonConvert.SerializeObject(new
            {
                LongToken = longSession.LongToken1,
                ShortToken = tokenAndClaimsPrincipal.Token,
                ShortTokenExpiration = tokenAndClaimsPrincipal.Expiration
            }, jsonSerializerSettings);

            httpContext.Response.Headers.Add(Headers.TokensHeaderName, json);

            return tokenAndClaimsPrincipal.ClaimsPrincipal;
        }


        private class TokenAndClaimsPrincipal
        {
            public string Token;
            public SunClaimsPrincipal ClaimsPrincipal;
            public DateTime Expiration;
        }

        private async Task<TokenAndClaimsPrincipal> CreateShortTokenAsync(
            User user, string lat2r, string lat2, long sessionId)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(TokenClaimNames.LongToken2Ran, lat2r),
                new Claim(JwtRegisteredClaimNames.Jti,
                    CryptoRandomizer.GetRandomString(DbColumnSizes.BlackListShortToken_TokenId))
            };

            var roleNames = await userManager.GetRolesAsync(user);

            foreach (var role in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var expiration = DateTime.UtcNow.AddMinutes(jweOptions.ShortTokenLiveTimeMinutes);

            var token = new JwtSecurityToken(
                claims: claims.ToArray(),
                expires: expiration);

            var claimsIdentity = new ClaimsIdentity(claims, "JwtShortToken");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return new TokenAndClaimsPrincipal
            {
                ClaimsPrincipal = new SunClaimsPrincipal(claimsPrincipal, rolesCache, sessionId, lat2),
                Token = cryptService.Crypt(CipherSecrets.ShortJwt, token.Payload.SerializeToJson()),
                Expiration = expiration
            };
        }

        public ClaimsPrincipal ReadShortToken(string token)
        {
            var tokenDecrypted = cryptService.Decrypt(CipherSecrets.ShortJwt, token);

            JwtSecurityToken jwtSecurityToken =
                new JwtSecurityToken(new JwtHeader(), JwtPayload.Deserialize(tokenDecrypted));

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(jwtSecurityToken.Claims, SunJwt.Scheme);

            if (jwtSecurityToken.ValidTo.Add(TokensExpiration.Delta) < DateTime.UtcNow)
                throw new Exception("Short token expires");

            return new ClaimsPrincipal(claimsIdentity);
        }


        private string CreateLong2Token(LongSession longSession, out string lat2r)
        {
            lat2r = CryptoRandomizer.GetRandomString(10);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, longSession.UserId.ToString()),
                new Claim(TokenClaimNames.LongToken2Ran, lat2r),
                new Claim(TokenClaimNames.LongToken2Db, longSession.LongToken2),
                new Claim(TokenClaimNames.SessionId, longSession.Id.ToString())
            };

            var token = new JwtSecurityToken(
                claims: claims.ToArray(),
                expires: longSession.ExpirationDate);

            return cryptService.Crypt(CipherSecrets.Long2Jwt, token.Payload.SerializeToJson());
        }

        public JwtSecurityToken ReadLong2Token(string token)
        {
            var tokenDecrypted = cryptService.Decrypt(CipherSecrets.Long2Jwt, token);

            var jwtSecurityToken = new JwtSecurityToken(new JwtHeader(), JwtPayload.Deserialize(tokenDecrypted));

            if (jwtSecurityToken.ValidTo.Add(TokensExpiration.Delta) < DateTime.UtcNow)
                throw new Exception("Long2 token expires");

            return jwtSecurityToken;
        }


        public void MakeLogoutCookiesAndHeaders(HttpResponse response)
        {
            response.Headers.Clear();

            response.Cookies.Delete(TokenClaimNames.LongToken2CoockiName,
                new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,
                    IsEssential = true
                });

            response.Headers.Add(Headers.TokensHeaderName, Headers.TokensExpireValue);
        }
    }
}
