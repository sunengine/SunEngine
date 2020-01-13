using System;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.Cache.Services;


namespace SunEngine.Core.Controllers
{
	/// <summary>
	/// Send all categories information controller
	/// </summary>
	public class MenuController : BaseController
	{
		protected readonly IMenuCache menuCache;


		public MenuController(
			IMenuCache menuCache,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.menuCache = menuCache;
		}

		[HttpPost]
		public virtual IActionResult GetAllMenuItems()
		{
			var menu = menuCache.GetMenu(User.Roles);
			return Json(menu);
		}
	}
}