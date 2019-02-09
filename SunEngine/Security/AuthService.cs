using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using LinqToDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SunEngine.Configuration.Options;
using SunEngine.Controllers;
using SunEngine.DataBase;
using SunEngine.Managers;
using SunEngine.Models;
using SunEngine.Services;
using SunEngine.Utils;

namespace SunEngine.Security
{
    public class AuthService : DbService
    {
        private readonly JwtOptions jwtOptions;
        private readonly MyUserManager userManager;
        private readonly GlobalOptions globalOptions;
        private readonly IEmailSender emailSender;
        private readonly IUrlHelperFactory urlHelperFactory;
        private readonly IActionContextAccessor accessor;
        private readonly ILogger logger;


        public AuthService(
            MyUserManager userManager,
            IEmailSender emailSender,
            DataBaseConnection db,
            IOptions<GlobalOptions> globalOptions,
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor accessor,
            IOptions<JwtOptions> jwtOptions,
            ILoggerFactory loggerFactory) : base(db)
        {
            this.jwtOptions = jwtOptions.Value;
            this.userManager = userManager;
            this.globalOptions = globalOptions.Value;
            this.emailSender = emailSender;
            this.urlHelperFactory = urlHelperFactory;
            this.accessor = accessor;
            logger = loggerFactory.CreateLogger<AuthController>();
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

        public class RegisterResult
        {
            public bool Succeeded;
            public ErrorViewModel Error;
        }

        public async Task<RegisterResult> RegisterAsync(NewUserViewModel model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Avatar = User.DefaultAvatar,
                Photo = User.DefaultAvatar
            };

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                await db.Users.Where(x => x.Id == user.Id).Set(x => x.Link, x => x.Id.ToString()).UpdateAsync();

                if (!result.Succeeded)
                    return new RegisterResult
                    {
                        Succeeded = false,
                        Error = new ErrorViewModel
                        {
                            ErrorsNames = result.Errors.Select(x => x.Code).ToArray(),
                            ErrorsTexts = result.Errors.Select(x => x.Description).ToArray()
                        }
                    };

                logger.LogInformation($"New user registered (id: {user.Id})");

                if (!user.EmailConfirmed)
                {
                    // Send email confirmation email
                    var confirmToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var (schema, host) = globalOptions.GetSchemaAndHostApi();

                    var Url = GetUrlHelper();

                    var emailConfirmUrl = Url.Action("Confirm", "Auth",
                        new {uid = user.Id, token = confirmToken},
                        schema, host);

                    try
                    {
                        await emailSender.SendEmailAsync(model.Email, "Please confirm your account",
                            $"Please confirm your account by clicking this <a href=\"{emailConfirmUrl}\">link</a>."
                        );
                    }
                    catch (Exception e)
                    {
                        return new RegisterResult
                        {
                            Succeeded = false,
                            Error = new ErrorViewModel
                            {
                                ErrorName = "Can not send email",
                                ErrorText = "Ошибка отправки email"
                            }
                        };
                    }


                    logger.LogInformation($"Sent email confirmation email (id: {user.Id})");
                }

                logger.LogInformation($"User logged in (id: {user.Id})");

                transaction.Complete();

                return new RegisterResult
                {
                    Succeeded = true
                };
            }
        }

        private IUrlHelper GetUrlHelper()
        {
            ActionContext context = accessor.ActionContext;
            return urlHelperFactory.GetUrlHelper(context);
        }
    }
}