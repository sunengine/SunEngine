using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Managers;
using SunEngine.Models;
using SunEngine.Security.Authentication;
using SunEngine.Stores;
using SunEngine.Stores.CacheModels;
using SunEngine.Utils;

namespace SunEngine.Controllers
{
    public abstract class BaseController : Controller
    {
        protected readonly MyUserManager userManager;
        protected readonly IRolesCache rolesCache;
        protected readonly IContentCache contentCache;
        protected readonly CacheKeyGenerator keyGenerator;

        protected BaseController(IServiceProvider serviceProvider)
        {
            contentCache = serviceProvider.GetRequiredService<IContentCache>();
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

        protected IActionResult JsonString(string json)
        {
            return Content(json, "application/json", Encoding.UTF8);
        }

        protected async Task<IActionResult> CacheContentAsync<T>(CategoryCached category, IEnumerable<int> categoryIdxes,
            Func<Task<T>> dataLoader)
        {
            var key = keyGenerator.ContentGenerateKey(ControllerName, ActionName, categoryIdxes);
            return await CacheContentAsync(category, key, dataLoader);
        }

        protected async Task<IActionResult> CacheContentAsync<T>(CategoryCached category, int categoryIdx,
            Func<Task<T>> dataLoader)
        {
            var key = keyGenerator.ContentGenerateKey(ControllerName, ActionName, categoryIdx);
            return await CacheContentAsync(category, key, dataLoader);
        }

        protected async Task<IActionResult> CacheContentAsync<T>(CategoryCached category, string key,
            Func<Task<T>> dataLoader)
        {
            string json;
            var normalizeKey = Normalizer.Normalize(key);
            if (category != null
                && category.IsCacheContent
                && !string.IsNullOrEmpty(json = contentCache.GetContent(normalizeKey)))
            {
                return JsonString(json);
            }

            var content = await dataLoader();
            json = WebJson.Serialize(content);
            contentCache.CacheContent(normalizeKey, json);
            return JsonString(json);
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