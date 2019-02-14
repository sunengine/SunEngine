using System;
using System.Net;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Controllers;
using SunEngine.Stores;

namespace SunEngine.Security.Filters
{
    public class IpSpamProtectionFilter : ActionFilterAttribute
    {
        private const string CacheKeyStart = "RFIP";
        
        public int TimeoutSeconds
        {
            set => timeout = TimeSpan.FromSeconds(value);
            get => (int)timeout.TotalSeconds;
        }

        protected TimeSpan timeout; 

        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            
            SpamProtectionCache spamProtectionCache =
                context.HttpContext.RequestServices.GetRequiredService<SpamProtectionCache>();

            BaseController controller = (BaseController) context.Controller;
            

            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            string controllerName = actionDescriptor?.ControllerTypeInfo.FullName;
            string actionName = actionDescriptor?.ActionName;

            var ip = controller.Request.HttpContext.Connection.RemoteIpAddress;
            
            string key = MakeKey(ip, controllerName, actionName);
            RequestFree requestFree = spamProtectionCache.Find(key);
            
            if (requestFree != null && requestFree.Working())
            {
                context.Result = controller.BadRequest(new ErrorViewModel
                {
                    ErrorName = "SpamProtection",
                    ErrorText = "Нельзя так часто делать запросы."
                });
            }

            SpamProtectionFilterTransfer temp = new SpamProtectionFilterTransfer()
            {
                Key = key,
                RequestFree = requestFree,
                SpamProtectionCache = spamProtectionCache
            };
            
            controller.ViewData[SpamProtectionFilterTransfer.ViewDataKey] = temp;
        }

        private static string MakeKey(IPAddress ip, string controllerName, string actionName)
        {
            return CacheKeyStart + "-" + ip + "-" + controllerName + "-" + actionName;
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            if (context.HttpContext.Response.StatusCode != 200)
                return;

            BaseController controller = (BaseController) context.Controller;

            var temp = (SpamProtectionFilterTransfer)controller.ViewData[SpamProtectionFilterTransfer.ViewDataKey];
            
            if (temp.RequestFree != null)
            {
                temp.RequestFree.UpdateDateTime(timeout);
            }
            else
            {
                var requestFree = new RequestFree(timeout);
                temp.SpamProtectionCache.Add(temp.Key, requestFree);
            }
        }
    }
    

  
}