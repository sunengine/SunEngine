using System;
using System.Net;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Controllers;
using SunEngine.Core.Errors;
using SunEngine.Core.Security;

namespace SunEngine.Core.Filters
{
    public class IpAndUserSpamProtectionFilter : ActionFilterAttribute
    {
        private const string CacheKeyStart = "RFIP";
        private readonly TimeSpan AdminTimeout =  new TimeSpan(0,0,5);
        
        public int IpTimeoutSeconds
        {
            set => ipTimeout = TimeSpan.FromSeconds(value);
            get => (int) ipTimeout.TotalSeconds;
        }

        public int UserTimeoutSeconds
        {
            set => userTimeout = TimeSpan.FromSeconds(value);
            get => (int) userTimeout.TotalSeconds;
        }

        public int TimeoutSeconds
        {
            set => ipTimeout = userTimeout = TimeSpan.FromSeconds(value);
            get => (int) ipTimeout.TotalSeconds;
        }

        protected TimeSpan ipTimeout;
        protected TimeSpan userTimeout;


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SpamProtectionCache spamProtectionCache =
                context.HttpContext.RequestServices.GetRequiredService<SpamProtectionCache>();

            BaseController controller = (BaseController) context.Controller;

            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            string controllerName = actionDescriptor?.ControllerTypeInfo.FullName;
            string actionName = actionDescriptor?.ActionName;

            var user = controller.User;

            var key = user.Identity.IsAuthenticated
                ? MakeKeyUser(user.UserId, controllerName, actionName)
                : MakeKeyIp(controller.Request.HttpContext.Connection.RemoteIpAddress, controllerName, actionName);

            RequestFree requestFree = spamProtectionCache.Find(key);

            if (requestFree != null && requestFree.Working())
                context.Result =
                    controller.BadRequest(ErrorView.SoftError("SpamProtection", "To rapid requests does not allowed"));

            SpamProtectionFilterTransfer temp = new SpamProtectionFilterTransfer
            {
                Key = key,
                RequestFree = requestFree,
                SpamProtectionCache = spamProtectionCache
            };

            controller.ViewData[SpamProtectionFilterTransfer.ViewDataKey] = temp;
        }

        private static string MakeKeyIp(IPAddress ip, string controllerName, string actionName)
        {
            return $"{CacheKeyStart}-{ip}-{controllerName}-{actionName}";
        }

        private static string MakeKeyUser(int userId, string controllerName, string actionName)
        {
            return $"{CacheKeyStart}-{userId}-{controllerName}-{actionName}";
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.HttpContext.Response.StatusCode != 200)
                return;

            BaseController controller = (BaseController) context.Controller;

            var temp = (SpamProtectionFilterTransfer) controller.ViewData[SpamProtectionFilterTransfer.ViewDataKey];

            var user = controller.User;
            TimeSpan thisTimeout;
            if (!user.Identity.IsAuthenticated)
                thisTimeout = ipTimeout;
            else if (!user.IsInRole(RoleNames.Admin))
                thisTimeout = userTimeout;
            else
                thisTimeout = AdminTimeout;
            
            if (temp.RequestFree != null)
                temp.RequestFree.UpdateDateTime(thisTimeout);
            else
                temp.SpamProtectionCache.Add(temp.Key, new RequestFree(thisTimeout));
        }
    }
}
