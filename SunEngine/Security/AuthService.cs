using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Flurl.Util;
using LinqToDB;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.Commons.Utils;
using SunEngine.Configuration.Options;
using SunEngine.Controllers;
using SunEngine.Security.Authentication;
using SunEngine.Services;

namespace SunEngine.Security
{
    public class AuthService : DbService
    {
        private readonly MyUserManager userManager;
        private readonly JwtOptions jwtOptions;
        private readonly ILogger logger;


        public AuthService(
            DataBaseConnection db,
            MyUserManager userManager,
            IOptions<JwtOptions> jwtOptions,
            ILoggerFactory loggerFactory) : base(db)
        {
            this.userManager = userManager;
            this.jwtOptions = jwtOptions.Value;
            logger = loggerFactory.CreateLogger<AuthController>();
        }

        private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public async Task<ClaimsPrincipal> RenewSecurityTokensAsync(HttpResponse response, int userId, LongSession longSession = null)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            return await RenewSecurityTokensAsync(response, user, longSession);
        }

        public async Task<ClaimsPrincipal> RenewSecurityTokensAsync(HttpResponse response, User user, LongSession longSession = null)
        {
            void GenerateTokens(LongSession longSession1)
            {
                longSession1.LongToken1 = CryptoRandomizer.GetRandomString(LongSession.LongToken1Length);
                longSession1.LongToken2 = CryptoRandomizer.GetRandomString(LongSession.LongToken2Length);
                longSession1.ExpirationDate = DateTime.UtcNow.AddDays(90);
            }


            if (longSession == null)
            {
                longSession = new LongSession
                {
                    UserId = user.Id,
                    DeviceInfo = ""
                };

                GenerateTokens(longSession);

                longSession.Id = await db.InsertWithInt64IdentityAsync(longSession);
            }
            else
            {
                GenerateTokens(longSession);
                await db.UpdateAsync(longSession);
            }


            var lat2Token = CreateLong2AuthToken(longSession, out string lat2r);

            response.Cookies.Append(
                "LAT2",
                lat2Token,
                new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,
                    IsEssential = true,
                    Expires = longSession.ExpirationDate
                }
            );

            TokenAndClaimsPrincipal tokenAndClaimsPrincipal = await GenerateShortAuthTokenAsync(user, lat2r);

            string json = JsonConvert.SerializeObject(new
            {
                LongToken = new
                {
                    Token = longSession.LongToken1,
                    Expiration = longSession.ExpirationDate.ToInvariantString()
                },
                ShortToken = tokenAndClaimsPrincipal.Token
            }, jsonSerializerSettings);

            response.Headers.Add("TOKENS", json);

            return tokenAndClaimsPrincipal.ClaimsPrincipal;
        }

        public string GenerateChangeEmailToken(User user, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKeyEmailChange));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, CryptoRandomizer.GetRandomString(16))
            };

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Issuer,
                claims: claims.ToArray(),
                expires: DateTime.Now.AddDays(3),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool ValidateChangeEmailToken(string token, out int userId, out string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKeyEmailChange));

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = new TokenValidationParameters()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = jwtOptions.Issuer,
                ValidAudience = jwtOptions.Issuer,
                IssuerSigningKey = key // The same key as the one that generate the token
            };

            var securityToken = tokenHandler.ValidateToken(token, validationParameters, out _);
            if (securityToken == null)
            {
                userId = 0;
                email = null;
                return false;
            }

            var jwt = tokenHandler.ReadJwtToken(token);
            email = jwt.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value;
            userId = int.Parse(jwt.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            return true;
        }

        internal class TokenAndClaimsPrincipal
        {
            public string Token;
            public ClaimsPrincipal ClaimsPrincipal;
        }

        private async Task<TokenAndClaimsPrincipal> GenerateShortAuthTokenAsync(User user, string lat2r)
        {
            // Generate and issue a JWT token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.ShortJwtSecurityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("LAT2R", lat2r),
                new Claim(JwtRegisteredClaimNames.Jti, CryptoRandomizer.GetRandomString(16))
            };

            var roleNames = await userManager.GetRolesAsync(user);

            foreach (var role in roleNames)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Issuer,
                claims: claims.ToArray(),
                expires: DateTime.UtcNow.AddMinutes(1), //DateTime.Now.AddDays(1),
                signingCredentials: credentials);

            return new TokenAndClaimsPrincipal
            {
                ClaimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims)),
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }

        public JwtSecurityToken ReadLongToken2(string token)
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

        public ClaimsPrincipal ReadShortToken(string token, out SecurityToken securityToken)
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

        private string CreateLong2AuthToken(LongSession longSession, out string lat2r)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.LongJwtSecurityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            lat2r = CryptoRandomizer.GetRandomString(10);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, longSession.UserId.ToString()),
                new Claim("LAT2R", lat2r),
                new Claim("LAT2", longSession.LongToken2),
                new Claim("ID", longSession.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Issuer,
                claims: claims.ToArray(),
                expires: longSession.ExpirationDate,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public void MakeLogoutCookiesAndHeaders(HttpResponse response)
        {
            response.Cookies.Append("LAT2", "",
                new CookieOptions
                {
                    Path = "/",
                    HttpOnly = true,
                    IsEssential = true,
                    Expires = DateTimeOffset.UtcNow.AddMonths(-1)
                });

            response.Headers.Add("TOKENSEXPIRE", "TRUE");
        }
    }
}