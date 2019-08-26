using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using LinqToDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Controllers;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors;
using SunEngine.Core.Models;
using SunEngine.Core.Security;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Managers
{
    public interface IAuthManager
    {
        Task<User> LoginAsync(string nameOrEmail, string password);
        Task LogoutAsync(int userId, long sessionId);
        Task RegisterAsync(NewUserArgs model);
        Task<bool> CheckUserNameInDbAsync(string userName);
    }


    public class AuthManager : DbService, IAuthManager
    {
        protected readonly SunUserManager userManager;
        protected readonly GlobalOptions globalOptions;
        protected readonly IEmailSenderService emailSenderService;
        protected readonly ILogger logger;
        protected readonly JweBlackListService jweBlackListService;


        public AuthManager(
            SunUserManager userManager,
            IEmailSenderService emailSenderService,
            DataBaseConnection db,
            IOptions<GlobalOptions> globalOptions,
            JweBlackListService jweBlackListService,
            ILoggerFactory loggerFactory) : base(db)
        {
            this.userManager = userManager;
            this.globalOptions = globalOptions.Value;
            this.emailSenderService = emailSenderService;
            this.jweBlackListService = jweBlackListService;
            logger = loggerFactory.CreateLogger<AccountController>();
        }


        public async Task<User> LoginAsync(string nameOrEmail, string password)
        {
            User user = await userManager.FindUserByNameOrEmailAsync(nameOrEmail);

            if (user == null || !await userManager.CheckPasswordAsync(user, password))
                throw new SunViewException(ErrorView.SoftError("UsernamePasswordInvalid",
                    "The username or password is invalid."));

            if (!await userManager.IsEmailConfirmedAsync(user))
                throw new SunViewException(ErrorView.SoftError("EmailNotConfirmed",
                    "You must have a confirmed email to log in."));

            if (await userManager.IsUserInRoleAsync(user.Id, RoleNames.Banned))
                throw new SunViewException(new ErrorView("UserBanned", "User is banned", ErrorType.System));

            return user;
        }

        public virtual async Task LogoutAsync(int userId, long sessionId)
        {
            await jweBlackListService.AddUserTokensToBlackListAsync(userId, new[] {sessionId});
            await db.LongSessions.Where(x => x.UserId == userId && x.Id == sessionId).DeleteAsync();
        }

        public virtual async Task RegisterAsync(NewUserArgs model)
        {
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Avatar = User.DefaultAvatar,
                Photo = User.DefaultAvatar,
                RegisteredDate = DateTime.UtcNow
            };

            using (db.BeginTransaction())
            {
                IdentityResult result = await userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                {
                    // If user already try to register but do not confirmed, try to update data

                    if (!result.Errors.Any(x => x.Code == "DuplicateEmail"))
                        throw new SunViewException(new ErrorView(result.Errors));

                    user = await userManager.FindByEmailAsync(model.Email);
                    if (user.EmailConfirmed)
                        throw new SunViewException(new ErrorView(result.Errors));

                    user.UserName = model.UserName;
                    user.PasswordHash = userManager.PasswordHasher.HashPassword(user, model.Password);

                    result = await userManager.UpdateAsync(user);

                    if (!result.Succeeded)
                        throw new SunViewException(new ErrorView(result.Errors));
                }
                else
                {
                    await db.Users.Where(x => x.Id == user.Id).Set(x => x.Link, x => x.Id.ToString())
                        .UpdateAsync();

                    logger.LogInformation($"New user registered (id: {user.Id})");
                }

                // Send email confirmation email
                var confirmToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

                var emailConfirmUrl = globalOptions.SiteApi
                    .AppendPathSegments("Auth", "ConfirmRegister")
                    .SetQueryParams(new {uid = user.Id, token = confirmToken});

                try
                {
                    await emailSenderService.SendEmailByTemplateAsync(
                        model.Email,
                        "register.html",
                        new Dictionary<string, string> {{"[link]", emailConfirmUrl}}
                    );
                }
                catch (Exception exception)
                {
                    throw new SunViewException(new ErrorView("EmailSendError", "Can not send email",
                        ErrorType.System, exception));
                }


                logger.LogInformation($"Sent email confirmation email (id: {user.Id})");

                db.CommitTransaction();
            }
        }

        public Task<bool> CheckUserNameInDbAsync(string userName)
        {
            var nameNormalized = Normalizer.Normalize(userName);
            return db.Users.AnyAsync(x => x.NormalizedUserName == nameNormalized);
        }
    }
}
