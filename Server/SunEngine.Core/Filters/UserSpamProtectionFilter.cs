using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Controllers;
using SunEngine.Core.Errors;
using SunEngine.Core.Security;

namespace SunEngine.Core.Filters
{
	public class UserSpamProtectionFilter : SpamProtectionFilterBase
	{
		private readonly TimeSpan AdminTimeout = new TimeSpan(0, 0, 5);
		protected override string CacheKey => "RFUSER";

		public override void OnActionExecuting(ActionExecutingContext context)
		{
			var user = context.HttpContext.User;
			if (!user.Identity.IsAuthenticated)
				context.Result = (context.Controller as BaseController)?.Unauthorized();

			base.OnActionExecuting(context);
		}

		protected override string GetClientId(BaseController controller)
		{
			return controller.User.UserId.ToString();
		}

		protected override TimeSpan CalcTimeout(BaseController controller)
		{
			return controller.User.IsInRole(RoleNames.Admin) ? AdminTimeout : blockTimeout;
		}
	}
}