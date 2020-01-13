using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Controllers;
using SunEngine.Core.Errors;
using SunEngine.Core.Services;

namespace SunEngine.Core.Filters
{
	public class CaptchaValidationFilter : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			CaptchaService captchaService =
				context.HttpContext.RequestServices.GetRequiredService<CaptchaService>();

			var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
			if (descriptor != null)
			{
				var parameters = descriptor.MethodInfo.GetParameters();

				var modelParameter = parameters.FirstOrDefault(x => x.ParameterType.IsSubclassOf(typeof(CaptchaArgs)));

				var model = (CaptchaArgs) context.ActionArguments[modelParameter.Name];

				if (!captchaService.VerifyToken(model.CaptchaToken, model.CaptchaText))
				{
					context.Result =
						((Controller) context.Controller).BadRequest(
							new Error("CaptchaValidationError", "Captcha text not valid", ErrorType.Soft));
				}

				base.OnActionExecuting(context);
			}
		}
	}
}