using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Flurl;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SunEngine.Commons.Configuration.Options;
using SunEngine.Commons.Controllers;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.Commons.Utils;

namespace SunEngine.Commons.Managers
{
    public interface IAccountManager
    {
        Task SendChangeEmailConfirmationMessageByEmailAsync(User user, string email);
        bool ValidateChangeEmailToken(string token, out int userId, out string email);
        Task<ServiceResult> ResetPasswordSendEmailAsync(User user);
    }

    public class AccountManager : DbService, IAccountManager
    {
        protected readonly JwtOptions jwtOptions;
        protected readonly SunUserManager userManager;
        protected readonly GlobalOptions globalOptions;
        protected readonly IEmailSenderService EmailSenderService;


        public AccountManager(
            SunUserManager userManager,
            IEmailSenderService emailSenderService,
            DataBaseConnection db,
            IOptions<GlobalOptions> globalOptions,
            IOptions<JwtOptions> jwtOptions) : base(db)
        {
            this.jwtOptions = jwtOptions.Value;
            this.userManager = userManager;
            this.globalOptions = globalOptions.Value;
            this.EmailSenderService = emailSenderService;
        }

        public virtual async Task<ServiceResult> ResetPasswordSendEmailAsync(User user)
        {
            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            var resetPasswordUrl = globalOptions.SiteApi
                .AppendPathSegments("Account", "ResetPasswordShowClientDialog")
                .SetQueryParams(new {uid = user.Id, token = resetToken});

            try
            {
                await EmailSenderService.SendEmailAsync(user.Email, "You can reset your password",
                    $"Reset password by clicking this <a href=\"{resetPasswordUrl}\">link</a>."
                );
            }
            catch (Exception)
            {
                ServiceResult.BadResult(new ErrorView ("EmailSendError","Server error. Can not send email."));
            }

            return ServiceResult.OkResult();
        }

        public virtual string GenerateChangeEmailToken(User user, string email)
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

        public virtual async Task SendChangeEmailConfirmationMessageByEmailAsync(User user, string email)
        {
            var emailToken = GenerateChangeEmailToken(user, email);

            var updateEmailUrl = globalOptions.SiteApi.AppendPathSegments("Account", "ConfirmChangeEmail")
                .SetQueryParam("token", emailToken);

            await EmailSenderService.SendEmailAsync(email, "Confirm your email",
                $"Confirm your email by clicking this <a href=\"{updateEmailUrl}\">link</a>.");
        }

        public virtual bool ValidateChangeEmailToken(string token, out int userId, out string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKeyEmailChange));

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
    }
}