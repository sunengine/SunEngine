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
using SunEngine.Security.Cryptography;
using SunEngine.Services;
using SunEngine.Utils;
    
namespace SunEngine.Security
{
    public interface IAccountService
    {
        Task<UserServiceResult> LoginAsync(string nameOrEmail, string password);
        string GenerateChangeEmailToken(User user, string email);
        bool ValidateChangeEmailToken(string token, out int userId, out string email);
        Task<ServiceResult> RegisterAsync(NewUserViewModel model);
    }

    public class AccountService : DbService, IAccountService
    {
        protected readonly JwtOptions jwtOptions;
        protected readonly MyUserManager userManager;
        protected readonly GlobalOptions globalOptions;
        protected readonly IEmailSender emailSender;
        protected readonly IUrlHelperFactory urlHelperFactory;
        protected readonly IActionContextAccessor accessor;
        protected readonly ILogger logger;


        public AccountService(
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
            logger = loggerFactory.CreateLogger<AccountController>();
        }

        public async Task<User> FindUserByNameOrEmailAsync(string nameOrEmail)
        {
            User user;
            if (EmailValidator.IsValidEmail(nameOrEmail))
            {
                user = await userManager.FindByEmailAsync(nameOrEmail)
                       ?? await userManager.FindByNameAsync(nameOrEmail); // if name is email like
            }
            else
            {
                user = await userManager.FindByNameAsync(nameOrEmail);
            }

            return user;
        }

        public async Task<UserServiceResult> LoginAsync(string nameOrEmail, string password)
        {
            User user = await FindUserByNameOrEmailAsync(nameOrEmail);

            if (user == null || !await userManager.CheckPasswordAsync(user, password))
            {
                return UserServiceResult.BadResult(new ErrorViewModel
                {
                    ErrorName = "username_password_invalid",
                    ErrorText = "The username or password is invalid."
                });
            }

            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                return UserServiceResult.BadResult(new ErrorViewModel
                {
                    ErrorName = "email_not_confirmed",
                    ErrorText = "You must have a confirmed email to log in."
                });
            }

            if (await userManager.IsUserInRoleAsync(user.Id, RoleNames.Banned))
            {
                return UserServiceResult.BadResult(new ErrorViewModel
                {
                    ErrorName = "user_banned",
                    ErrorText = "Error" // Что бы не провоцировать пользователя словами что он забанен
                });
            }

            return UserServiceResult.OkResult(user);
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


        

        public virtual bool ValidateChangeEmailToken(string token, out int userId, out string email)
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


        public virtual async Task<ServiceResult> RegisterAsync(NewUserViewModel model)
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
                    return new ServiceResult
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

                    var emailConfirmUrl = Url.Action("Confirm", "Account",
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
                        return new ServiceResult
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

                return new ServiceResult
                {
                    Succeeded = true
                };
            }
        }

        protected IUrlHelper GetUrlHelper()
        {
            ActionContext context = accessor.ActionContext;
            return urlHelperFactory.GetUrlHelper(context);
        }
    }
}