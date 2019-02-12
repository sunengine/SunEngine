using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Filters;
using SunEngine.Managers;
using SunEngine.Services;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
    public class CaptchaController : BaseController
    {
        protected readonly CaptchaService captchaService;

        public CaptchaController(
            MyUserManager userManager, 
            IUserGroupStore userGroupStore,
            CaptchaService captchaService) : base(userGroupStore, userManager)
        {
            this.captchaService = captchaService;
        }

        [AllowAnonymous]
        [IpSpamProtectionFilter(TimeoutSeconds=20)]
        public virtual IActionResult GetCaptchaKey()
        {
            var token = captchaService.MakeCryptedCaptchaToken();
            return Content(token);
        }

        [AllowAnonymous]
        [Produces("image/jpeg")]
        public virtual FileStreamResult CaptchaImage(string token)
        {
            var text = captchaService.GetTextFromToken(token);
            var captcha = captchaService.MakeCaptchaImage(text);

            return File(captcha, "image/jpeg");
        }
    }
}