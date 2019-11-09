using System;
using System.Threading.Tasks;
using Flurl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Errors;
using SunEngine.Core.Filters;
using SunEngine.Core.Managers;
using SunEngine.Core.Models;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Controllers
{
    /// <summary>
    /// Change and reset password, change email controller
    /// </summary>
    public class AccountController : BaseController
    {
        private readonly IOptionsMonitor<GlobalOptions> globalOptions;
        private readonly IAccountManager accountManager;

        public AccountController(
            IAccountManager accountManager,
            IOptionsMonitor<GlobalOptions> globalOptions,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.globalOptions = globalOptions;
            this.accountManager = accountManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [CaptchaValidationFilter]
        public async Task<IActionResult> ResetPasswordSendEmail(ResetPasswordArgs model)
        {
            User user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(new ErrorView("UserWithThisEmailNotFound", "User with this email not found.",
                    ErrorType.System));

            await accountManager.ResetPasswordSendEmailAsync(user);

            return Ok();
        }

        /// <summary>
        /// Show client dialog to change password
        /// </summary>
        [AllowAnonymous]
        [HttpGet] // Goes here FromMail
        public async Task<IActionResult> ResetPasswordShowClientDialog(string uid, string token)
        {
            var user = await userManager.FindByIdAsync(uid);
            if (user == null)
                return Redirect(Flurl.Url.Combine(globalOptions.CurrentValue.SiteUrl, "Account/ResetPasswordFailed".ToLower()));

            if (await userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "ResetPassword", token))
            {
                return Redirect(Flurl.Url.Combine(globalOptions.CurrentValue.SiteUrl, "Account/ResetPasswordSetNew".ToLower())
                    .SetQueryParams(new {uid = uid, token = token}));
            }

            return Redirect(Flurl.Url.Combine(globalOptions.CurrentValue.SiteUrl, "Account/ResetPasswordFailed".ToLower()));
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> ResetPasswordSetNew(string uid, string token, string newPassword)
        {
            var user = await userManager.FindByIdAsync(uid);
            var result = await userManager.ResetPasswordAsync(user, token, newPassword);
            if (result.Succeeded)
                return Ok();

            return BadRequest(ErrorView.ServerError());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(string password, string email)
        {
            email = email.Trim();

            if (!EmailValidator.IsValid(email))
                return BadRequest(ErrorView.SoftError("EmailInvalid", "Email not valid"));

            var user = await GetUserAsync();

            if (!await userManager.CheckPasswordAsync(user, password))
                return BadRequest(ErrorView.SoftError("PasswordInvalid", "Password not valid"));

            if (await userManager.CheckEmailInDbAsync(email, user.Id))
                return BadRequest(ErrorView.SoftError("EmailAlreadyTaken", "Email already registered"));

            await accountManager.SendChangeEmailConfirmationMessageByEmailAsync(user, email);

            return Ok();
        }

        [AllowAnonymous]
        [HttpGet] // Goes here FromMail
        public async Task<IActionResult> ConfirmChangeEmail(string token)
        {
            try
            {
                if (!accountManager.ValidateChangeEmailToken(token, out int userId, out string email)
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

            return Redirect(Flurl.Url.Combine(globalOptions.CurrentValue.SiteUrl, "Account/ChangeEmailResult?result=ok".ToLower()));

            IActionResult Error()
            {
                return Redirect(Flurl.Url.Combine(globalOptions.CurrentValue.SiteUrl, "Account/ChangeEmailResult?result=error")
                    .ToLower());
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string passwordOld, string passwordNew)
        {
            var user = await GetUserAsync();

            var result = await userManager.ChangePasswordAsync(user, passwordOld, passwordNew);
            if (result.Succeeded)
                return Ok();

            return BadRequest(new ErrorView(result.Errors));
        }
    }
}
