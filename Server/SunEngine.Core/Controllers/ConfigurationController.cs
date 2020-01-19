using System;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.Cache.Services;

namespace SunEngine.Core.Controllers
{
	public class ConfigurationController : BaseController
	{
		protected readonly IDynamicConfigCache dynamicConfigCache;

		public ConfigurationController(
			IDynamicConfigCache dynamicConfigCache,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.dynamicConfigCache = dynamicConfigCache;
		}

		[HttpPost]
		public IActionResult GetDynamicConfig()
		{
			return JsonString(dynamicConfigCache.DynamicConfigCacheJson);
		}
	}
}