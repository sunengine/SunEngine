using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SunEngine.Configuration.Options;
using SunEngine.Controllers;
using SunEngine.Managers;
using SunEngine.Models;
using SunEngine.Utils;

namespace SunEngine.Security
{
    public class AuthService
    {
        private readonly JwtOptions jwtOptions;
        private readonly MyUserManager userManager;
        private readonly GlobalOptions globalOptions;
        private readonly IEmailSender emailSender;
        private readonly  IUrlHelperFactory urlHelperFactory;
        private readonly IActionContextAccessor accessor;
        
        public AuthService(
            MyUserManager userManager, 
            IEmailSender emailSender,
            IOptions<GlobalOptions> globalOptions,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor accessor,
            IOptions<JwtOptions> jwtOptions)
        {
            this.jwtOptions = jwtOptions.Value;
            this.userManager = userManager;
            this.globalOptions = globalOptions.Value;
            this.emailSender = emailSender;
            this.urlHelperFactory = urlHelperFactory;
            this.accessor = accessor;
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
        
        
        public async Task SendChangeEmailConfirmationMessageByEmailAsync(User user, string email)
        {
            var urlHelper = GetUrlHelper();
            
            var emailToken = await userManager.GenerateChangeEmailTokenAsync(user, email);

            var (schema, host) = globalOptions.GetSchemaAndHostApi();

            var updateEmailUrl = urlHelper.Action("ConfirmEmail", "Auth",
                new {token = emailToken}, schema, host);
           
            await emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Confirm your email by clicking this <a href=\"{updateEmailUrl}\">link</a>.");          
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
        
        private IUrlHelper GetUrlHelper()
        {
            ActionContext context = accessor.ActionContext;
            return urlHelperFactory.GetUrlHelper(context);
        }
    }
}