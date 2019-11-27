using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Presenters;
using SunEngine.Admin.Services;
using SunEngine.Core.Models;

namespace SunEngine.Admin.Controllers
{
    public class ConfigurationAdminController : BaseAdminController
    {
        protected readonly IConfigurationManager configurationManager;
        protected readonly IConfigurationPresenter configurationPresenter;
        protected readonly IConfigurationRoot configurationRoot;
        protected readonly ConfigurationAdminService configurationAdminService;

        public ConfigurationAdminController(
            IConfigurationManager configurationManager,
            ConfigurationAdminService configurationAdminService,
            IConfigurationPresenter configurationPresenter,
            IConfigurationRoot configurationRoot,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.configurationRoot = configurationRoot;
            this.configurationManager = configurationManager;
            this.configurationAdminService = configurationAdminService;
            this.configurationPresenter = configurationPresenter;
        }

        [HttpPost]
        public IActionResult UploadConfiguration()
        {
            var items = Request.Form.Select(x => new ConfigurationItem
            {
                Name = x.Key,
                Value = x.Value
            }).ToList();

            configurationManager.UploadConfigurationItems(items);
            configurationAdminService.UpdateClientScripts();
            configurationRoot.Reload();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> LoadConfiguration()
        {
            var items = await configurationPresenter.LoadConfigurationAsync();

            return Ok(items);
        }

        [HttpPost]
        public IActionResult GetEnums()
        {
            var items = configurationPresenter.GetEnums();

            return Json(items);
        }
    }
}