using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Cache.CacheModels;
using SunEngine.Core.Cache.CachePolicy;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Errors;
using SunEngine.Core.Managers;
using SunEngine.Core.Models;
using SunEngine.Core.Security;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Controllers
{
    /// <summary>
    /// Base class for all controllers on the site
    /// </summary>
    public abstract class BaseController : Controller
    {
        protected readonly SunUserManager userManager;
        protected readonly IRolesCache rolesCache;
        protected readonly ICachePolicy cachePolicy;
        protected readonly IContentCache contentCache;
        protected readonly CacheKeyGenerator keyGenerator;

        protected BaseController(IServiceProvider serviceProvider)
        {
            contentCache = serviceProvider.GetRequiredService<IContentCache>();
            cachePolicy = serviceProvider.GetRequiredService<ICachePolicy>();
            rolesCache = serviceProvider.GetRequiredService<IRolesCache>();
            userManager = serviceProvider.GetRequiredService<SunUserManager>();
            keyGenerator = serviceProvider.GetRequiredService<CacheKeyGenerator>();
        }

        protected string ControllerName => ControllerContext.ActionDescriptor.ControllerName;

        protected string ActionName => ControllerContext.ActionDescriptor.ActionName;

        protected string UserOrIpKey =>
            User != null
                ? "u"+User.UserId
                : Request.HttpContext.Connection.RemoteIpAddress.ToString();

        private SunClaimsPrincipal _user;

        public new SunClaimsPrincipal User
        {
            get
            {
                if (_user == null)
                {
                    SunClaimsPrincipal sunClaimsPrincipal = base.User as SunClaimsPrincipal;
                    _user = sunClaimsPrincipal ?? new SunClaimsPrincipal(base.User, rolesCache);
                }

                return _user;
            }
        }

        public new UnauthorizedObjectResult Unauthorized()
        {
            return base.Unauthorized(ErrorView.Unauthorized());
        }

        public new BadRequestObjectResult BadRequest()
        {
            return base.BadRequest(ErrorView.BadRequest());
        }
        
        public Task<User> GetUserAsync()
        {
            return userManager.FindByIdAsync(User.UserId.ToString());
        }

        public IActionResult JsonString(string json)
        {
            return Content(json, "application/json", Encoding.UTF8);
        }

        public async Task<IActionResult> CacheContentAsync<T>(CategoryCached category, IEnumerable<int> categoryIds,
            Func<Task<T>> dataLoader, int? page = null)
        {
            var key = keyGenerator.ContentGenerateKey(ControllerName, ActionName, page, categoryIds);
            return await CacheContentAsync(category, key, dataLoader, page);
        }

        public async Task<IActionResult> CacheContentAsync<T>(
            CategoryCached category, 
            int categoryId,
            Func<Task<T>> dataLoader, 
            int? page = null)
        {
            var key = keyGenerator.ContentGenerateKey(ControllerName, ActionName, page, categoryId);
            return await CacheContentAsync(category, key, dataLoader, page);
        }

        protected async Task<IActionResult> CacheContentAsync<T>(
            CategoryCached category, 
            string key,
            Func<Task<T>> dataLoader,
            int? page)
        {
            if (!cachePolicy.CanCache(category, page))
                return Json(await dataLoader());

            string json;
            if (!string.IsNullOrEmpty(json = contentCache.GetContent(key)))
                return JsonString(json);

            var content = await dataLoader();
            json = SunJson.Serialize(content);
            contentCache.CacheContent(key, json);
            return JsonString(json);
        }

        protected override void Dispose(bool disposing)
        {
            userManager.Dispose();
            base.Dispose(disposing);
        }
    }

    
}
