using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SunEngine.Core.Cache.Services;

namespace SunEngine.Admin.Controllers
{
    public class CacheAdminController : BaseAdminController
    {
        protected readonly IComponentsCache componentsCache;
        protected readonly ICategoriesCache categoriesCache;
        protected readonly IMenuCache menuCache;
        protected readonly SpamProtectionCache spamProtectionCache;
        protected readonly IMailTemplatesCache mailTemplatesCache;
        protected readonly IConfigurationRoot configurationRoot;

        public CacheAdminController(
            IComponentsCache componentsCache,
            ICategoriesCache categoriesCache,
            IMenuCache menuCache,
            SpamProtectionCache spamProtectionCache,
            IMailTemplatesCache mailTemplatesCache,
            IConfigurationRoot configurationRoot,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.componentsCache = componentsCache;
            this.categoriesCache = categoriesCache;
            this.menuCache = menuCache;
            this.spamProtectionCache = spamProtectionCache;
            this.mailTemplatesCache = mailTemplatesCache;
            this.configurationRoot = configurationRoot;
        }

        public IActionResult ResetAllCache()
        {
            componentsCache.Initialize();
            categoriesCache.Initialize();
            menuCache.Initialize();
            rolesCache.Initialize();
            contentCache.Reset();
            spamProtectionCache.Reset();
            mailTemplatesCache.Initialize();
            configurationRoot.Reload();

            return Ok();
        }
    }
}