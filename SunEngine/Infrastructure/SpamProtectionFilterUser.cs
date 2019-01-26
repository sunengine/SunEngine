using System;
using System.Collections.Generic;
using System.Threading;
using Flurl.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Commons.Models;
using SunEngine.Controllers;
using SunEngine.Stores;

namespace SunEngine.Infrastructure
{
    public class SpamProtectionFilterUser : ActionFilterAttribute
    {
        private const string CacheKeyStart = "RFUSER";

        
        public int TimeoutSeconds
        {
            set => timeout = TimeSpan.FromSeconds(value);
            get => (int)timeout.TotalSeconds;
        }

        protected TimeSpan timeout; 

        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            SpamProtectionStore spamProtectionStore =
                context.HttpContext.RequestServices.GetRequiredService<SpamProtectionStore>();

            BaseController controller = (BaseController) context.Controller;

            var user = controller.User;

            if (!user.Identity.IsAuthenticated)
            {
                throw new Exception("This user can not make post requests");
            }

            var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
            string controllerName = actionDescriptor?.ControllerTypeInfo.FullName;
            string actionName = actionDescriptor?.ActionName;

            string key = MakeKey(user.UserId, controllerName, actionName);
            RequestFree requestFree = spamProtectionStore.Find(key);
            
            
            
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
                SpamProtectionStore = spamProtectionStore
            };
            
            controller.ViewData[SpamProtectionFilterTransfer.ViewDataKey] = temp;
        }

        private static string MakeKey(int userId, string controllerName, string actionName)
        {
            return CacheKeyStart + "-" + userId + "-" + controllerName + "-" + actionName;
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
                temp.SpamProtectionStore.Add(temp.Key, requestFree);
            }
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
        public SpamProtectionStore SpamProtectionStore;
    }

    public class RequestFree
    {

        private DateTime dateTimeTil;

        public RequestFree(TimeSpan timeout)
        {
            dateTimeTil = DateTime.Now.Add(timeout);
        }

        public void UpdateDateTime(TimeSpan timeout)
        {
            dateTimeTil = DateTime.Now.Add(timeout);
        }

        public bool Working()
        {
            return dateTimeTil >= DateTime.Now;
        }
    }
}