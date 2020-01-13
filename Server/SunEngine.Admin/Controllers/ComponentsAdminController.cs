using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Presenters;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Models;

namespace SunEngine.Admin.Controllers
{
	public class ComponentsAdminController : BaseAdminController
	{
		protected readonly IComponentsAdminPresenter componentsAdminPresenter;
		protected readonly IComponentsAdminManager componentsAdminManager;
		protected readonly IComponentsCache componentsCache;

		public ComponentsAdminController(
			IComponentsAdminPresenter componentsAdminPresenter,
			IComponentsAdminManager componentsAdminManager,
			IComponentsCache componentsCache,
			IServiceProvider serviceProvider) : base(serviceProvider: serviceProvider)
		{
			this.componentsAdminPresenter = componentsAdminPresenter;
			this.componentsAdminManager = componentsAdminManager;
			this.componentsCache = componentsCache;
		}

		[HttpPost]
		public async Task<IActionResult> GetAllComponents()
		{
			var rez = await componentsAdminPresenter.GetComponentsAsync();
			return Ok(rez);
		}

		[HttpPost]
		public async Task<IActionResult> GetComponent(string name)
		{
			var component = await componentsAdminPresenter.GetComponentAsync(name);
			return Ok(component);
		}

		[HttpPost]
		public async Task<IActionResult> AddComponent([FromBody] Component component)
		{
			await componentsAdminManager.CreateComponentAsync(component);

			componentsCache.Initialize();

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> UpdateComponent([FromBody] Component component)
		{
			await componentsAdminManager.UpdateComponentAsync(component);

			componentsCache.Initialize();

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> DeleteComponent(int componentId)
		{
			await componentsAdminManager.DeleteComponentAsync(componentId);

			componentsCache.Initialize();

			return Ok();
		}
	}
}