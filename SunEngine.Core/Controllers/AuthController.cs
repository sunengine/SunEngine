using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using LinqToDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Filters;
using SunEngine.Core.Managers;
using SunEngine.Core.Security;

namespace SunEngine.Core.Controllers
{
    /// <summary>
    /// Login, logout, register controller
    /// </summary>
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        private readonly JweService jweService;
        private readonly DataBaseConnection db;
        private readonly GlobalOptions globalOptions;
        private readonly IAuthManager authManager;


        public AuthController(
            DataBaseConnection db,
            JweService jweService,
            IAuthManager authManager,
            IOptions<GlobalOptions> globalOptions,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.globalOptions = globalOptions.Value;
            this.db = db;
            this.jweService = jweService;
            this.authManager = authManager;
        }


        [HttpPost]
        public async Task<IActionResult> Login(string nameOrEmail, string password)
        {
            var user = await authManager.LoginAsync(nameOrEmail, password);

            await jweService.RenewSecurityTokensAsync(HttpContext, user);

            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await authManager.LogoutAsync(User.UserId, User.SessionId);

            jweService.MakeLogoutCookiesAndHeaders(Response);

            return Ok();
        }

        [HttpPost]
        [CaptchaValidationFilter]
        public async Task<IActionResult> Register(NewUserArgs model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await authManager.RegisterAsync(model);

            return Ok();
        }

        public async Task<IActionResult> CheckUserNameInDb(string userName)
        {
            return Ok(new {yes = await authManager.CheckUserNameInDbAsync(userName)});
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmRegister(string uid, string token)
        {
            var user = await userManager.FindByIdAsync(uid);

            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var confirmResult = await userManager.ConfirmEmailAsync(user, token);
                    if (confirmResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, RoleNames.Registered);

                        transaction.Complete();
                        return Redirect(Flurl.Url.Combine(globalOptions.SiteUrl, "Auth/RegisterEmailResult?result=ok")
                            .ToLower());
                    }
                }
                catch
                {
                    // ignored
                }
            }

            return Redirect(Flurl.Url.Combine(globalOptions.SiteUrl,
                "Auth/RegisterEmailResult?result=error".ToLower()));
        }
    }

    public class NewUserArgs : CaptchaArgs
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

    public class CaptchaArgs
    {
        [Required] public string CaptchaToken { get; set; }

        [Required] public string CaptchaText { get; set; }
    }

    public class ResetPasswordArgs : CaptchaArgs
    {
        [Required] [EmailAddress] public string Email { get; set; }
    }
}
