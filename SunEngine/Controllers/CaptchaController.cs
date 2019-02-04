using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.Filters;
using SunEngine.Services;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
    public class CaptchaController : BaseController
    {

        private readonly CaptchaService captchaService;

        public CaptchaController(
            MyUserManager userManager, 
            IUserGroupStore userGroupStore,
            CaptchaService captchaService) : base(userGroupStore, userManager)
        {
            this.captchaService = captchaService;
        }

        [AllowAnonymous]
        [IpSpamProtectionFilter(TimeoutSeconds=20)]
        public IActionResult GetCaptchaKey()
        {
            var token = captchaService.MakeCryptedCaptchaToken();
            return Content(token);
        }

        [AllowAnonymous]
        [Produces("image/jpeg")]
        public FileStreamResult CaptchaImage(string token)
        {
            var text = captchaService.GetTextFromToken(token);
            var captcha = captchaService.MakeCaptchaImage(text);

            return File(captcha, "image/jpeg");
        }
    }
}