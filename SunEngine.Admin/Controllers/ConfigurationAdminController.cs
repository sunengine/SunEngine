using System;
using System.Security.Policy;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Presenters;

namespace SunEngine.Admin.Controllers
{
    public class ConfigurationAdminController : BaseAdminController
    {
        protected readonly IConfigurationManager configurationManager;
        protected readonly IConfigurationPresenter configurationPresenter;
        
        public ConfigurationAdminController(
            IConfigurationManager configurationManager,
            IConfigurationPresenter configurationPresenter,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.configurationManager = configurationManager;
            this.configurationPresenter = configurationPresenter;
        }

        [HttpPost]
        public IActionResult UploadConfiguration()
        {
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