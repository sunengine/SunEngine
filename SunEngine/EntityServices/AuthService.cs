using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
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

        public async Task<string> GenerateTokenAsync(int userId)
        {
            var user = await userManager.FindByIdAsync(userId.ToString());
            return await GenerateTokenAsync(user);
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            // Generate and issue a JWT token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                //new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, CryptService.CryptoProvider.GetRandomString(16))
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
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

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
                new Claim(JwtRegisteredClaimNames.Jti, CryptService.CryptoProvider.GetRandomString(16))
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

            var securityToken = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            if (securityToken == null)
            {
                userId = 0;
                email = null;
                return false;
            }

            var jwt = tokenHandler.ReadJwtToken(token);
            email = jwt.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value;
            userId = int.Parse(jwt.Claims.First(x=>x.Type == ClaimTypes.NameIdentifier).Value);
            return true;
        }
        
        public Task<bool> CheckEmailInDbAsync(string email, int userId)
        {
            return db.Users.AnyAsync(x => x.NormalizedEmail == email.ToUpper() && x.Id != userId );
        }

    }

}