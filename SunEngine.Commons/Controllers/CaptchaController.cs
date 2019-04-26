using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.Filters;
using SunEngine.Commons.Services;

namespace SunEngine.Commons.Controllers
{
    /// <summary>
    /// Spam protection captcha controller
    /// </summary>
    public class CaptchaController : BaseController
    {
        protected readonly CaptchaService captchaService;

        public CaptchaController(
            CaptchaService captchaService,
            IServiceProvider serviceProvider) : base(serviceProvider)
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