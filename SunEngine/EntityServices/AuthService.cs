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
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Utils;
using SunEngine.Controllers;
using SunEngine.Options;
using SunEngine.Services;

namespace SunEngine.EntityServices
{
    public class AuthService : DbService
    {
        private readonly UserManager<User> userManager;
        private readonly JwtOptions jwtOptions;
        private readonly ILogger logger;


        public AuthService(
            DataBaseConnection db,
            UserManager<User> userManager,
            IOptions<JwtOptions> jwtOptions,
            ILoggerFactory loggerFactory) : base(db)
        {
            this.userManager = userManager;
            this.jwtOptions = jwtOptions.Value;
            logger = loggerFactory.CreateLogger<AuthController>();
        }

        /*public async Task<string> GenerateTokenAsync(int userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            return await GenerateTokenAsync(user);
        }*/

        

        public Task ChangeEmailAsync(int userId, string email)
        {
            return db.Users.Where(x => x.Id == userId).Set(x => x.Email, email)
                .Set(x => x.NormalizedEmail, email.ToUpper())
                .UpdateAsync();
        }

        public async Task<string> GenerateChangeEmailTokenAsync(User user, string email)
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

        public Task<bool> CheckEmailInDbAsync(string email, int userId)
        {
            return db.Users.AnyAsync(x => x.NormalizedEmail == email.ToUpper() && x.Id != userId);
        }

        public LongSession FindLongSession(LongSession longSession)
        {
            return db.LongSessions.FirstOrDefault(x => x.UserId == longSession.UserId &&
                                                       x.LongToken1 == longSession.LongToken1 &&
                                                       x.LongToken2 == longSession.LongToken2);
        }

        private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public async Task RenewSecurityTokensAsync(HttpResponse response, int userId, LongSession longSession = null)
        {
            var user = await db.Users.FirstOrDefaultAsync(x => x.Id == userId);
            await RenewSecurityTokensAsync(response, user, longSession);
        }

        public async Task RenewSecurityTokensAsync(HttpResponse response, User user, LongSession longSession = null)
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

                await db.InsertAsync(longSession);
            }
            else
            {
                GenerateTokens(longSession);
                await db.UpdateAsync(longSession);
            }


            //response.Headers.Add("LAT1", longSession.LongToken1);
            //response.Headers.Add("LATEXP", longSession.ExpirationDate.ToInvariantString());

            var lat2Token = CreateLong2AuthToken(longSession.LongToken2, longSession.ExpirationDate, user.Id, out string lat2r);

            response.Cookies.Append(
                "LAT2",
                lat2Token,
                new CookieOptions
                {
                    Path = "/",
                    HttpOnly = false,
                    IsEssential = true,
                    Expires = longSession.ExpirationDate
                }
            );

            var shortToken = await GenerateShortAuthTokenAsync(user,lat2r);


            string json = JsonConvert.SerializeObject(new
            {
                LongToken = new
                {
                    Token = longSession.LongToken1,
                    Expiration = longSession.ExpirationDate.ToInvariantString()
                },
                ShortToken = shortToken
            }, jsonSerializerSettings);

            response.Headers.Add("TOKENS", json);
        }

        public string CreateLong2AuthToken(string lat2, DateTime expireDate, int userId, out string lat2r)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtOptions.LongJwtSecurityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            lat2r = CryptoRandomizer.GetRandomString(10);
            
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim("LAT2R", lat2r),
                new Claim("LAT2", lat2)
            };

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Issuer,
                claims: claims.ToArray(),
                expires: expireDate,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        public async Task<string> GenerateShortAuthTokenAsync(User user, string lat2r)
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

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}