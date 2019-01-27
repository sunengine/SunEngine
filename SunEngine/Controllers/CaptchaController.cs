using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.Infrastructure;
using SunEngine.Services;

namespace SunEngine.Controllers
{
    public class CaptchaController : BaseController
    {

        private readonly CaptchaService captchaService;

        public CaptchaController(
            MyUserManager userManager, 
            CaptchaService captchaService) : base(userManager)
        {
            this.captchaService = captchaService;
        }

        [AllowAnonymous]
        [SpamProtectionFilterIp(TimeoutSeconds=20)]
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