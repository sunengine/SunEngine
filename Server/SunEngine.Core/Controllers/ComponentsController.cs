using System;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.Cache.Services;

namespace SunEngine.Core.Controllers
{
	/// <summary>
	/// Get cached component data controller
	/// </summary>
	public class ComponentsController : BaseController
	{
		protected readonly IComponentsCache componentsCache;

		public ComponentsController(
			IComponentsCache componentsCache,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.componentsCache = componentsCache;
		}

		[HttpPost]
		public IActionResult GetAllComponents()
		{
			var rez = componentsCache.GetClientComponents(User.Roles);
			return Ok(rez);
		}
	}
}