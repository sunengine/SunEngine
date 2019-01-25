using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.Models;
using SunEngine.Services;

namespace SunEngine.Controllers
{
    public class CaptchaController : BaseController
    {

        private readonly CaptchaService captchaService;

        public CaptchaController(
            UserManager<User> userManager, 
            CaptchaService captchaService) : base(userManager)
        {
            this.captchaService = captchaService;
        }

        [AllowAnonymous]
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