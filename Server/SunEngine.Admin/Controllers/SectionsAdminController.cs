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
		protected readonly ISectionsAdminPresenter sectionsAdminPresenter;
		protected readonly ISectionsAdminManager sectionsAdminManager;
		protected readonly ISectionsCache sectionsCache;

		public SectionsAdminController(
			ISectionsAdminPresenter sectionsAdminPresenter,
			ISectionsAdminManager sectionsAdminManager,
			ISectionsCache sectionsCache,
			IServiceProvider serviceProvider) : base(serviceProvider: serviceProvider)
		{
			this.sectionsAdminPresenter = sectionsAdminPresenter;
			this.sectionsAdminManager = sectionsAdminManager;
			this.sectionsCache = sectionsCache;
		}

		[HttpPost]
		public async Task<IActionResult> GetAllSections()
		{
			var rez = await sectionsAdminPresenter.GetSectionsAsync();
			return Ok(rez);
		}

		[HttpPost]
		public async Task<IActionResult> GetComponent(string name)
		{
			var component = await sectionsAdminPresenter.GetSectionAsync(name);
			return Ok(component);
		}

		[HttpPost]
		public async Task<IActionResult> AddSection([FromBody] Section section)
		{
			await sectionsAdminManager.CreateSectionAsync(section);

			sectionsCache.Initialize();

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateSection([FromBody] Section section)
		{
			await sectionsAdminManager.UpdateSectionAsync(section);

			sectionsCache.Initialize();

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> DeleteSection(int componentId)
		{
			await sectionsAdminManager.DeleteSectionAsync(componentId);

			sectionsCache.Initialize();

			return Ok();
		}
	}
}