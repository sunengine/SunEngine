using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Commons.Cache;
using SunEngine.Commons.Cache.CacheModels;
using SunEngine.Commons.Cache.CachePolicy;
using SunEngine.Commons.Managers;
using SunEngine.Commons.Models;
using SunEngine.Commons.Security.Authentication;
using SunEngine.Commons.Utils;

namespace SunEngine.Commons.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly MyUserManager userManager;
        protected readonly IRolesCache rolesCache;
        protected readonly ICachePolicy cachePolicy;
        protected readonly IContentCache contentCache;
        protected readonly CacheKeyGenerator keyGenerator;

        protected BaseController(IServiceProvider serviceProvider)
        {
            contentCache = serviceProvider.GetRequiredService<IContentCache>();
            cachePolicy = serviceProvider.GetRequiredService<ICachePolicy>();
            rolesCache = serviceProvider.GetRequiredService<IRolesCache>();
            userManager = serviceProvider.GetRequiredService<MyUserManager>();
            keyGenerator = serviceProvider.GetRequiredService<CacheKeyGenerator>();
        }

        protected string ControllerName
        {
            get => ControllerContext.ActionDescriptor.ControllerName;
        }

        protected string ActionName
        {
            get => ControllerContext.ActionDescriptor.ActionName;
        }

        private MyClaimsPrincipal _user;

        public new MyClaimsPrincipal User
        {
            get
            {
                if (_user == null)
                {
                    MyClaimsPrincipal myClaimsPrincipal = base.User as MyClaimsPrincipal;
                    _user = myClaimsPrincipal ?? new MyClaimsPrincipal(base.User, rolesCache);
                }

                return _user;
            }
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
            Func<Task<T>> dataLoader,  int page = 0)
        {
            var key = keyGenerator.ContentGenerateKey(ControllerName, ActionName, page, categoryIds);
            return await CacheContentAsync(category, key, dataLoader);
        }

        public async Task<IActionResult> CacheContentAsync<T>(CategoryCached category, int categoryId,
            Func<Task<T>> dataLoader, int page = 0)
        {
            var key = keyGenerator.ContentGenerateKey(ControllerName, ActionName, page, categoryId);
            return await CacheContentAsync(category, key, dataLoader);
        }

        protected async Task<IActionResult> CacheContentAsync<T>(CategoryCached category, string key,
            Func<Task<T>> dataLoader)
        {
            if (!cachePolicy.CanCache(category))
                return Json(await dataLoader());

            string json;
            if (category != null
                && category.IsCacheContent
                && !string.IsNullOrEmpty(json = contentCache.GetContent(key)))
            {
                return JsonString(json);
            }

            var content = await dataLoader();
            json = WebJson.Serialize(content);
            contentCache.CacheContent(key, json);
            return JsonString(json);
        }

        protected override void Dispose(bool disposing)
        {
            userManager.Dispose();
            base.Dispose(disposing);
        }
    }

    public class ErrorViewModel
    {
        public string ErrorName { get; set; }
        public string ErrorText { get; set; }
        public string[] ErrorsNames { get; set; }
        public string[] ErrorsTexts { get; set; }
    }
}