using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.Filters;
using SunEngine.Core.Services;

namespace SunEngine.Core.Controllers
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
		[IpSpamProtectionFilter(TimeoutSeconds = 5, AllowedRequestCount = 4, RestrictSeconds = 60)]
		public virtual IActionResult GetCaptchaKey()
		{
			var token = captchaService.MakeCaptchaToken();
			return Content(token);
		}

		[AllowAnonymous]
		[Produces("image/jpeg")]
		[IpSpamProtectionFilter(TimeoutSeconds = 5, AllowedRequestCount = 4, RestrictSeconds = 60)]
		public virtual FileStreamResult CaptchaImage(string token)
		{
			var text = captchaService.GetAnswerByToken(token);
			var captcha = captchaService.MakeCaptchaImage(text);

			return File(captcha, "image/jpeg");
		}
	}
}