using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Presenters;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Models;

namespace SunEngine.Admin.Controllers
{
	public class SectionsAdminController : BaseAdminController
	{
		protected readonly ISectionsAdminPresenter SectionsAdminPresenter;
		protected readonly ISectionsAdminManager SectionsAdminManager;
		protected readonly ISectionsCache SectionsCache;

		public SectionsAdminController(
			ISectionsAdminPresenter SectionsAdminPresenter,
			ISectionsAdminManager SectionsAdminManager,
			ISectionsCache SectionsCache,
			IServiceProvider serviceProvider) : base(serviceProvider: serviceProvider)
		{
			this.SectionsAdminPresenter = SectionsAdminPresenter;
			this.SectionsAdminManager = SectionsAdminManager;
			this.SectionsCache = SectionsCache;
		}

		[HttpPost]
		public async Task<IActionResult> GetAllSections()
		{
			var rez = await SectionsAdminPresenter.GetSectionsAsync();
			return Ok(rez);
		}

		[HttpPost]
		public async Task<IActionResult> GetComponent(string name)
		{
			var component = await SectionsAdminPresenter.GetComponentAsync(name);
			return Ok(component);
		}

		[HttpPost]
		public async Task<IActionResult> AddComponent([FromBody] Section section)
		{
			await SectionsAdminManager.CreateComponentAsync(section);

			SectionsCache.Initialize();

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateComponent([FromBody] Section section)
		{
			await SectionsAdminManager.UpdateComponentAsync(section);

			SectionsCache.Initialize();

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> DeleteComponent(int componentId)
		{
			await SectionsAdminManager.DeleteComponentAsync(componentId);

			SectionsCache.Initialize();

			return Ok();
		}
	}
}