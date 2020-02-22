using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Presenters;
using SunEngine.Admin.Services;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Models;

namespace SunEngine.Admin.Controllers
{
	public class ConfigurationAdminController : BaseAdminController
	{
		protected readonly IConfigurationAdminManager ConfigurationAdminManager;
		protected readonly IConfigurationAdminPresenter ConfigurationAdminPresenter;
		protected readonly IConfigurationRoot configurationRoot;
		protected readonly ConfigurationAdminService configurationAdminService;
		protected readonly IHostingEnvironment env;
		protected readonly IDynamicConfigCache dynamicConfigCache;

		public ConfigurationAdminController(
			IConfigurationAdminManager configurationAdminManager,
			ConfigurationAdminService configurationAdminService,
			IConfigurationAdminPresenter configurationAdminPresenter,
			IConfigurationRoot configurationRoot,
			IDynamicConfigCache dynamicConfigCache,
			IHostingEnvironment env,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.env = env;
			this.dynamicConfigCache = dynamicConfigCache;
			this.configurationRoot = configurationRoot;
			this.ConfigurationAdminManager = configurationAdminManager;
			this.configurationAdminService = configurationAdminService;
			this.ConfigurationAdminPresenter = configurationAdminPresenter;
		}

		[HttpPost]
		public IActionResult UploadConfiguration()
		{
			var items = Request.Form.Select(x => new ConfigurationItem
			{
				Name = x.Key,
				Value = x.Value
			}).ToList();

			ConfigurationAdminManager.UploadConfigurationItems(items);

			configurationRoot.Reload();

			dynamicConfigCache.Initialize();

			configurationAdminService.UpdateClientScripts();

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> LoadConfiguration()
		{
			var configurationView = await ConfigurationAdminPresenter.LoadConfigurationAsync();

			return Ok(configurationView);
		}
		
	}
}