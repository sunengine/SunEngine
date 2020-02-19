using System;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.Cache.Services;

namespace SunEngine.Core.Controllers
{
	/// <summary>
	/// Get cached component data controller
	/// </summary>
	public class SectionsController : BaseController
	{
		protected readonly ISectionsCache SectionsCache;

		public SectionsController(
			ISectionsCache SectionsCache,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.SectionsCache = SectionsCache;
		}

		[HttpPost]
		public IActionResult GetAllSections()
		{
			var rez = SectionsCache.GetClientSections(User.Roles);
			return Ok(rez);
		}
	}
}