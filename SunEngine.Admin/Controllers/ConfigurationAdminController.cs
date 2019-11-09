using System;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Presenters;
using SunEngine.Core.Configuration;
using SunEngine.Core.Models;

namespace SunEngine.Admin.Controllers
{
    public class ConfigurationAdminController : BaseAdminController
    {
        protected readonly IConfigurationManager configurationManager;
        protected readonly IConfigurationPresenter configurationPresenter;
        protected readonly ConfigDbProvider configDbProvider;
        public ConfigurationAdminController(
            IConfigurationManager configurationManager,
            IConfigurationPresenter configurationPresenter,
            ConfigDbProvider configDbProvider,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.configDbProvider = configDbProvider;
            this.configurationManager = configurationManager;
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
            configDbProvider.Load();
            
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> LoadConfiguration()
        {
            var items = await configurationPresenter.LoadConfigurationAsync();

            return Ok(items);
        }
    }
}