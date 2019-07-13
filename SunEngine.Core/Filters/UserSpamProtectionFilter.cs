using System;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Controllers;
using SunEngine.Core.Errors;
using SunEngine.Core.Security;

namespace SunEngine.Core.Filters
{
    public class UserSpamProtectionFilter : ActionFilterAttribute
    {
        private const string CacheKeyStart = "RFUSER";
        private readonly TimeSpan AdminTimeout = new TimeSpan(0, 0, 5);


        public int TimeoutSeconds
        {
            set => timeout = TimeSpan.FromSeconds(value);
            get => (int) timeout.TotalSeconds;
        }

        protected TimeSpan timeout;


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SpamProtectionCache spamProtectionCache =
                context.HttpContext.RequestServices.GetRequiredService<SpamProtectionCache>();

            BaseController controller = (BaseController) context.Controller;

            var user = controller.User;

            if (!user.Identity.IsAuthenticated)
                context.Result = controller.Unauthorized();

            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            string controllerName = actionDescriptor?.ControllerTypeInfo.FullName;
            string actionName = actionDescriptor?.ActionName;

            string key = MakeKey(user.UserId, controllerName, actionName);
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

        private static string MakeKey(int userId, string controllerName, string actionName)
        {
            return string.Join("-", CacheKeyStart, userId, controllerName, actionName);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.HttpContext.Response.StatusCode != 200)
                return;

            BaseController controller = (BaseController) context.Controller;

            var temp = (SpamProtectionFilterTransfer) controller.ViewData[SpamProtectionFilterTransfer.ViewDataKey];

            var thisTimeout = controller.User.IsInRole(RoleNames.Admin) ? AdminTimeout : timeout;


            if (temp.RequestFree != null)
                temp.RequestFree.UpdateDateTime(thisTimeout);
            else
                temp.SpamProtectionCache.Add(temp.Key, new RequestFree(thisTimeout));
        }
    }

    /// <summary>
    /// Object to transfer data between OnActionExecuting and OnResultExecuted
    /// </summary>
    public class SpamProtectionFilterTransfer
    {
        public const string ViewDataKey = "SpamProtectionFilterTransfer";

        public string Key;
        public RequestFree RequestFree;
        public SpamProtectionCache SpamProtectionCache;
    }

    public class RequestFree
    {
        private DateTime dateTimeTil;

        public RequestFree(TimeSpan timeout)
        {
            dateTimeTil = DateTime.UtcNow.Add(timeout);
        }

        public void UpdateDateTime(TimeSpan timeout)
        {
            dateTimeTil = DateTime.UtcNow.Add(timeout);
        }

        public bool Working()
        {
            return dateTimeTil >= DateTime.UtcNow;
        }
    }
}
