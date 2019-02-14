using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc;
using Flurl;
using LinqToDB;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using SunEngine.Configuration.Options;
using SunEngine.DataBase;
using SunEngine.Managers;
using SunEngine.Models;
using SunEngine.Security;
using SunEngine.Security.Authorization;
using SunEngine.Security.Captcha;
using SunEngine.Security.Filters;
using SunEngine.Services;
using SunEngine.Stores;
using SunEngine.Stores.Models;
using SunEngine.Utils;

namespace SunEngine.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly IEmailSender emailSender;
        private readonly ILogger logger;
        private readonly JwtService jwtService;
        private readonly DataBaseConnection db;
        private readonly GlobalOptions globalOptions;
        private readonly IAccountService accountService;

        public AccountController(
            MyUserManager userManager,
            IEmailSender emailSender,
            DataBaseConnection db,
            ILoggerFactory loggerFactory,
            JwtService jwtService,
            IAccountService accountService,
            IOptions<GlobalOptions> globalOptions,
            IUserGroupStore userGroupStore) : base(userGroupStore, userManager)
        {
            this.globalOptions = globalOptions.Value;
            this.emailSender = emailSender;
            logger = loggerFactory.CreateLogger<AccountController>();
            this.db = db;
            this.jwtService = jwtService;
            this.accountService = accountService;
        }


        [AllowAnonymous]
        [Produces("application/json")]
        public async Task<IActionResult> Login(string nameOrEmail, string password)
        {
            var result = await accountService.LoginAsync(nameOrEmail,password);

            if (result.Failed)
            {
                return BadRequest(result.Error);
            }
            
            await jwtService.RenewSecurityTokensAsync(Response, result.user);

            return Ok();
        }

        public async Task<IActionResult> Logout()
        {
            int userId = User.UserId;
            long sessionId = User.SessionId;
            await db.LongSessions.Where(x => x.UserId == userId && x.Id == sessionId).DeleteAsync();

            jwtService.MakeLogoutCookiesAndHeaders(Response);

            return Ok();
        }


        [AllowAnonymous]
        [CaptchaValidationFilter]
        public async Task<IActionResult> Register(NewUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await accountService.RegisterAsync(model);
            if (!result.Succeeded)
                return BadRequest(result.Error);

            return Ok();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string passwordOld, string passwordNew)
        {
            var user = await GetUserAsync();

            var result = await userManager.ChangePasswordAsync(user, passwordOld, passwordNew);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(
                new ErrorViewModel {ErrorsTexts = result.Errors.Select(x => x.Description).ToArray()});
        }

        [HttpPost]
        [AllowAnonymous]
        [CaptchaValidationFilter]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            User user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new ErrorViewModel {ErrorText = "User with this email not found."});
            }

            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            var (schema, host) = globalOptions.GetSchemaAndHostApi();

            var resetPasswordUrl = Url.Action("ChangePasswordFromResetDialog", "Account",
                new {uid = user.Id, token = resetToken}, schema, host);
            try
            {
                await emailSender.SendEmailAsync(user.Email, "You can reset your password",
                    $"Reset password by clicking this <a href=\"{resetPasswordUrl}\">link</a>."
                );
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorViewModel {ErrorText = "Server error. Can not send email."});
            }

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ChangePasswordFromReset(string uid, string token, string newPassword)
        {
            var user = await userManager.FindByIdAsync(uid);
            var result = await userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest(new ErrorViewModel {ErrorText = "Server error. Something goes wrong."});
        }

        /// <summary>
        /// Show Interface to change password
        /// </summary>
        [AllowAnonymous]
        public async Task<IActionResult> ChangePasswordFromResetDialog(string uid, string token)
        {
            var user = await userManager.FindByIdAsync(uid);
            if (user == null)
            {
                return Redirect(Flurl.Url.Combine(globalOptions.SiteUrl, "auth/ResetPasswordFailed".ToLower()));
            }

            if (await userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", token))
            {
                return Redirect(Flurl.Url.Combine(globalOptions.SiteUrl, "auth/SetNewPasswordFromReset".ToLower())
                    .SetQueryParams(new {uid = uid, token = token}));
            }

            return Redirect(Flurl.Url.Combine(globalOptions.SiteUrl, "auth/ResetPasswordFailed".ToLower()));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Confirm(string uid, string token)
        {
            var user = await userManager.FindByIdAsync(uid);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var confirmResult = await userManager.ConfirmEmailAsync(user, token);
                    if (confirmResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, RoleStored.UserGroupRegistered);

                        transaction.Complete();
                        return Redirect(Flurl.Url.Combine(globalOptions.SiteUrl, "auth/emailconfirmed?result=ok"));
                    }
                }
                catch
                {
                    // ignored
                }
            }

            return Redirect(Flurl.Url.Combine(globalOptions.SiteUrl, "auth/emailconfirmed?result=error"));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(string password, string email)
        {
            email = email.Trim();

            if (!EmailValidator.IsValidEmail(email))
            {
                return BadRequest(new ErrorViewModel {ErrorText = "Email not valid"});
            }

            var user = await GetUserAsync();

            if (!await userManager.CheckPasswordAsync(user, password))
            {
                return BadRequest(new ErrorViewModel {ErrorText = "Password not valid"});
            }

            if (await userManager.CheckEmailInDbAsync(email, user.Id))
            {
                return BadRequest(new ErrorViewModel {ErrorText = "Email already registered"});
            }

            await accountService.SendChangeEmailConfirmationMessageByEmailAsync(user, email);

            return Ok();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            try
            {
                if (!accountService.ValidateChangeEmailToken(token, out int userId, out string email)
                    || await userManager.CheckEmailInDbAsync(email, User.UserId))
                {
                    return Error();
                }

                await userManager.ChangeEmailAsync(userId, email);
            }
            catch
            {
                return Error();
            }

            return Redirect(Flurl.Url.Combine(globalOptions.SiteUrl, "auth/emailconfirmed?result=ok"));

            IActionResult Error()
            {
                return Redirect(Flurl.Url.Combine(globalOptions.SiteUrl, "auth/emailconfirmed?result=error"));
            }
        }

        
    }

    public class NewUserViewModel : CaptchaViewModel
    {
        [Required]
        [MinLength(3)]
        [MaxLength(DbColumnSizes.Users_UserName)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(DbColumnSizes.Users_Email)]
        public string Email { get; set; }

        [Required] [MinLength(6)] public string Password { get; set; }
    }

    public class CaptchaViewModel
    {
        [Required] public string CaptchaToken { get; set; }

        [Required] public string CaptchaText { get; set; }
    }

    public class ResetPasswordViewModel : CaptchaViewModel
    {
        [Required] [EmailAddress] public string Email { get; set; }
    }
}