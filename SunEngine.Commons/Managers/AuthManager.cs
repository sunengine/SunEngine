using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Flurl;
using LinqToDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SunEngine.Commons.Configuration.Options;
using SunEngine.Commons.Controllers;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Security;
using SunEngine.Commons.Services;

namespace SunEngine.Commons.Managers
{
    public interface IAuthManager
    {
        Task<UserServiceResult> LoginAsync(string nameOrEmail, string password);
        Task<ServiceResult> RegisterAsync(NewUserArgs model);
    }

    public class AuthManager : DbService, IAuthManager
    {
        protected readonly SunUserManager userManager;
        protected readonly GlobalOptions globalOptions;
        protected readonly IEmailSenderService EmailSenderService;
        protected readonly ILogger logger;


        public AuthManager(
            SunUserManager userManager,
            IEmailSenderService emailSenderService,
            DataBaseConnection db,
            IOptions<GlobalOptions> globalOptions,
            ILoggerFactory loggerFactory) : base(db)
        {
            this.userManager = userManager;
            this.globalOptions = globalOptions.Value;
            this.EmailSenderService = emailSenderService;
            logger = loggerFactory.CreateLogger<AccountController>();
        }


        public async Task<UserServiceResult> LoginAsync(string nameOrEmail, string password)
        {
            User user = await userManager.FindUserByNameOrEmailAsync(nameOrEmail);

            if (user == null || !await userManager.CheckPasswordAsync(user, password))
            {
                return UserServiceResult.BadResult(
                    ErrorView.SoftError("UsernamePasswordInvalid", "The username or password is invalid."));
            }

            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                return UserServiceResult.BadResult(
                    ErrorView.SoftError("EmailNotConfirmed", "You must have a confirmed email to log in."));
            }

            if (await userManager.IsUserInRoleAsync(user.Id, RoleNames.Banned))
            {
                return UserServiceResult.BadResult(new ErrorView("UserBanned", "User is banned"));
            }

            return UserServiceResult.OkResult(user);
        }

        public virtual async Task<ServiceResult> RegisterAsync(NewUserArgs model)
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
                    return ServiceResult.BadResult(new ErrorView(result.Errors));

                logger.LogInformation($"New user registered (id: {user.Id})");

                if (!user.EmailConfirmed)
                {
                    // Send email confirmation email
                    var confirmToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var emailConfirmUrl = globalOptions.SiteApi.AppendPathSegments("Auth", "ConfirmRegister")
                        .SetQueryParams(new {uid = user.Id, token = confirmToken});

                    try
                    {
                        await EmailSenderService.SendEmailAsync(model.Email, "Please confirm your account",
                            $"Please confirm your account by clicking this <a href=\"{emailConfirmUrl}\">link</a>."
                        );
                    }
                    catch (Exception exception)
                    {
                        return ServiceResult.BadResult(
                            new ErrorView("EmailSendError", "Can not send email", exception));
                    }


                    logger.LogInformation($"Sent email confirmation email (id: {user.Id})");
                }

                logger.LogInformation($"User logged in (id: {user.Id})");

                transaction.Complete();

                return ServiceResult.OkResult();
            }
        }
    }
}