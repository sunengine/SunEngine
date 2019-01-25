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
using Microsoft.Extensions.Logging;
using SunEngine.Commons.Models;
using SunEngine.Options;
using Microsoft.AspNetCore.Identity;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.StoreModels;
using SunEngine.EntityServices;
using SunEngine.Infrastructure;
using SunEngine.Services;

namespace SunEngine.Controllers
{
    [Authorize]
    public class AuthController : BaseController
    {
        private readonly IEmailSender emailSender;
        private readonly ILogger logger;
        private readonly AuthService authService;
        private readonly DataBaseConnection db;
        private readonly GlobalOptions globalOptions;
        private readonly CaptchaService captchaService;

        public AuthController(
            UserManager<User> userManager,
            IEmailSender emailSender,
            DataBaseConnection db,
            ILoggerFactory loggerFactory,
            AuthService authService,
            CaptchaService captchaService,
            IOptions<GlobalOptions> globalOptions) : base(userManager)
        {
            this.globalOptions = globalOptions.Value;
            this.emailSender = emailSender;
            logger = loggerFactory.CreateLogger<AuthController>();
            this.db = db;
            this.authService = authService;
            this.captchaService = captchaService;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        public async Task<IActionResult> Login(string nameOrEmail, string password)
        {
            User user = null;

            // Ensure there is such user
            if (nameOrEmail.Contains('@')) // TODO check user names to block names with @
            {
                user = await userManager.FindByEmailAsync(nameOrEmail);
            }
            else
            {
                user = await userManager.FindByNameAsync(nameOrEmail);
            }

            if (user == null || !await userManager.CheckPasswordAsync(user, password))
            {
                return BadRequest(new ErrorViewModel
                {
                    ErrorName = "username_password_invalid",
                    ErrorText = "The username or password is invalid."
                });
            }

            // Ensure the email is confirmed.
            if (!await userManager.IsEmailConfirmedAsync(user))
            {
                return BadRequest(new ErrorViewModel
                {
                    ErrorName = "email_not_confirmed",
                    ErrorText = "You must have a confirmed email to log in."
                });
            }

            logger.LogInformation($"User logged in (id: {user.Id})");

            string token = await authService.GenerateTokenAsync(user);

            return Ok(new TokenViewModel {Token = token});
        }

        [AllowAnonymous]
        public async Task<IActionResult> Register(NewUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!captchaService.VerifyToken(model.CaptchaToken,model.CaptchaText))
            {
                return BadRequest("TokenError");
            }
            
            var user = new User
            {
                UserName = model.UserName,
                Email = model.Email,
                Avatar = SunEngine.Commons.Models.User.DefaultAvatar,
                Photo = SunEngine.Commons.Models.User.DefaultAvatar
            };

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                IdentityResult result = await userManager.CreateAsync(user, model.Password);
                await db.Users.Where(x => x.Id == user.Id).Set(x => x.Link, x => x.Id.ToString()).UpdateAsync();

                if (result.Succeeded)
                {
                    logger.LogInformation($"New user registered (id: {user.Id})");

                    if (!user.EmailConfirmed)
                    {
                        // Send email confirmation email
                        var confirmToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

                        var emailConfirmUrl = Url.Action("Confirm", "Auth",
                            new {uid = user.Id, token = confirmToken},
                            this.Request.Scheme);

                        await emailSender.SendEmailAsync(model.Email, "Please confirm your account",
                            $"Please confirm your account by clicking this <a href=\"{emailConfirmUrl}\">link</a>."
                        );


                        logger.LogInformation($"Sent email confirmation email (id: {user.Id})");
                    }

                    logger.LogInformation($"User logged in (id: {user.Id})");

                    transaction.Complete();

                    return Ok();
                }

                return BadRequest(new ErrorsViewModel
                {
                    ErrorsNames = result.Errors.Select(x => x.Code).ToArray(),
                    ErrorsTexts = result.Errors.Select(x => x.Description).ToArray()
                });
            }
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
            else
            {
                return BadRequest(
                    new ErrorsViewModel {ErrorsTexts = result.Errors.Select(x => x.Description).ToArray()});
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(string email)
        {
            User user = await userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new ErrorViewModel {ErrorText = "User with this email not found."});
            }

            var resetToken = await userManager.GeneratePasswordResetTokenAsync(user);

            var resetPasswordUrl = Url.Action("ChangePasswordFromResetDialog", "Auth",
                new {uid = user.Id, token = resetToken}, this.Request.Scheme);
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
            else
            {
                return BadRequest(new ErrorViewModel {ErrorText = "Server error. Something goes wrong."});
            }
        }

        /// <summary>
        /// Show Interface to change password
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="token"></param>
        /// <returns></returns>
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
            else
            {
                return Redirect(Flurl.Url.Combine(globalOptions.SiteUrl, "auth/ResetPasswordFailed".ToLower()));
            }
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
                        await userManager.AddToRoleAsync(user, UserGroup.UserGroupRegistered);

                        transaction.Complete();
                        return Redirect(Flurl.Url.Combine(globalOptions.SiteUrl, "auth/emailconfirmed?result=ok"));
                    }
                }
                catch
                {
                    
                }
            }

            return Redirect(Flurl.Url.Combine(globalOptions.SiteUrl, "auth/emailconfirmed?result=error"));
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangeEmail(string password, string email)
        {
            email = email.Trim();

            EmailAddressAttribute emailValidator = new EmailAddressAttribute();
            if (!emailValidator.IsValid(email))
            {
                return BadRequest(new ErrorViewModel {ErrorText = "Email not valid"});
            }

            var user = await GetUserAsync();

            if (!await userManager.CheckPasswordAsync(user, password))
            {
                return BadRequest(new ErrorViewModel {ErrorText = "Password not valid"});
            }

            if (await authService.CheckEmailInDbAsync(email, user.Id))
            {
                return BadRequest(new ErrorViewModel {ErrorText = "Email already registered"});
            }

            var emailToken = await authService.GenerateChangeEmailTokenAsync(user, email);

            var updateEmailUrl = Url.Action("ConfirmEmail", "Auth",
                new {token = emailToken}, this.Request.Scheme);
            try
            {
                await emailSender.SendEmailAsync(user.Email, "Confirm your email",
                    $"Confirm your email by clicking this <a href=\"{updateEmailUrl}\">link</a>."
                );
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorViewModel {ErrorText = "Server error. Can not send email."});
            }


            return Ok();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string token)
        {
            try
            {
                if (!authService.ValidateChangeEmailToken(token, out int userId, out string email)
                    || await authService.CheckEmailInDbAsync(email, User.UserId))
                {
                    return Error();
                }

                await authService.ChangeEmailAsync(userId, email);
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

    public class NewUserViewModel
    {
        [Required] public string UserName { get; set; }

        [Required] [EmailAddress] public string Email { get; set; }


        [Required] [MinLength(6)] public string Password { get; set; }
        
        [Required] public string CaptchaToken { get; set; }
        
        [Required] public string CaptchaText { get; set; }

    }

    public class TokenViewModel
    {
        public string Token { get; set; }
    }
}