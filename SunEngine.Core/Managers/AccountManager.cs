using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Flurl;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors;
using SunEngine.Core.Models;
using SunEngine.Core.Security;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Managers
{
    public interface IAccountManager
    {
        Task SendChangeEmailConfirmationMessageByEmailAsync(User user, string email);
        bool ValidateChangeEmailToken(string token, out int userId, out string email);
        Task ResetPasswordSendEmailAsync(User user);
    }

    public class AccountManager : DbService, IAccountManager
    {
        protected readonly JweOptions jweOptions;
        protected readonly SunUserManager userManager;
        protected readonly GlobalOptions globalOptions;
        protected readonly ICryptService cryptService;
        protected readonly IEmailSenderService emailSenderService;


        public AccountManager(
            SunUserManager userManager,
            IEmailSenderService emailSenderService,
            DataBaseConnection db,
            ICryptService cryptService,
            IOptions<GlobalOptions> globalOptions,
            IOptions<JweOptions> jwtOptions) : base(db)
        {
            this.jweOptions = jwtOptions.Value;
            this.userManager = userManager;
            this.globalOptions = globalOptions.Value;
            this.emailSenderService = emailSenderService;
            this.cryptService = cryptService;
        }

        public virtual async Task ResetPasswordSendEmailAsync(User user)
        {
            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            var resetPasswordUrl = globalOptions.SiteApi
                .AppendPathSegments("Account", "ResetPasswordShowClientDialog")
                .SetQueryParams(new {uid = user.Id, token = resetToken});

            try
            {
                await emailSenderService.SendEmailByTemplateAsync(
                    user.Email,
                    "reset-password.html",
                    new Dictionary<string, string>{{"[resetPassUrl]", resetPasswordUrl}}
                );
            }
            catch (Exception exception)
            {
                throw new SunViewException(new ErrorView ("EmailSendError","Server error. Can not send email.", ErrorType.System, exception));
            }
        }

        public virtual string GenerateChangeEmailToken(User user, string email)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti, CryptoRandomizer.GetRandomString(16))
            };

            var token = new JwtSecurityToken(
                claims: claims.ToArray(),
                expires: DateTime.UtcNow.AddDays(3));
            
            cryptService.Crypt(CipherSecrets.EmailChange,token.Payload.SerializeToJson());
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        
        public virtual bool ValidateChangeEmailToken(string token, out int userId, out string email)
        {
            try
            {
                var tokenDecrypted = cryptService.Decrypt(CipherSecrets.EmailChange, token);

                var jwtSecurityToken = new JwtSecurityToken(new JwtHeader(), JwtPayload.Deserialize(tokenDecrypted));

                email = jwtSecurityToken.Claims.First(x => x.Type == JwtRegisteredClaimNames.Email).Value;
                userId = int.Parse(jwtSecurityToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);

                if (jwtSecurityToken.ValidTo.Add(TokensExpiration.Delta) < DateTime.UtcNow)
                    return false;
            }
            catch
            {
                email = null;
                userId = 0;
                return false;
            }

            return true;
        }

        public virtual async Task SendChangeEmailConfirmationMessageByEmailAsync(User user, string email)
        {
            var emailToken = GenerateChangeEmailToken(user, email);

            var updateEmailUrl = globalOptions.SiteApi.AppendPathSegments("Account", "ConfirmChangeEmail")
                .SetQueryParam("token", emailToken);
            
            await emailSenderService.SendEmailByTemplateAsync(
                email,
                "email-change.html",
                new Dictionary<string, string> {{"[link]", updateEmailUrl}}
            );
        }
    }
}
