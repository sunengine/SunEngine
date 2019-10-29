using System;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.Cache.Services;

namespace SunEngine.Admin.Controllers
{
    public class CacheAdminController : BaseAdminController
    {
        protected readonly IComponentsCache componentsCache;
        protected readonly ICategoriesCache categoriesCache;
        protected readonly IMenuCache menuCache;
        //protected readonly IRolesCache rolesCache;
        //protected readonly IContentCache contentCache;
        protected readonly SpamProtectionCache spamProtectionCache;
        protected readonly IMailTemplatesCache mailTemplatesCache;


        public CacheAdminController(
            IComponentsCache componentsCache,
            ICategoriesCache categoriesCache,
            IMenuCache menuCache,
            //IRolesCache rolesCache,
            //IContentCache contentCache,
            SpamProtectionCache spamProtectionCache,
            IMailTemplatesCache mailTemplatesCache,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.componentsCache = componentsCache;
            this.categoriesCache = categoriesCache;
            this.menuCache = menuCache;
            //this.rolesCache = rolesCache;
            //this.contentCache = contentCache;
            this.spamProtectionCache = spamProtectionCache;
            this.mailTemplatesCache = mailTemplatesCache;
        }

        public IActionResult ResetAllCache()
        {
            componentsCache.Reset();
            categoriesCache.Reset();
            menuCache.Reset();
            rolesCache.Reset();
            contentCache.Reset();
            spamProtectionCache.Reset();
            mailTemplatesCache.Reset();

            return Ok();
        }
    }
}