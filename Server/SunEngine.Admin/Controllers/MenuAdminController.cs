using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Presenters;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Models;

namespace SunEngine.Admin.Controllers
{
	public class MenuAdminController : BaseAdminController
	{
		protected readonly IMenuAdminManager menuAdminManager;
		protected readonly IMenuAdminPresenter menuAdminPresenter;
		protected readonly IMenuCache menuCache;

		public MenuAdminController(
			IMenuAdminManager menuAdminManager,
			IMenuAdminPresenter menuAdminPresenter,
			IMenuCache menuCache,
			IServiceProvider serviceProvider) : base(serviceProvider)
		{
			this.menuAdminManager = menuAdminManager;
			this.menuAdminPresenter = menuAdminPresenter;
			this.menuCache = menuCache;
		}

		[HttpPost]
		public async Task<IActionResult> GetMenuItems()
		{
			var menuItems = await menuAdminPresenter.GetMenuItemsAsync();
			return Ok(menuItems);
		}

		[HttpPost]
		public async Task<IActionResult> GetMenuItem(int id)
		{
			var menuItem = await menuAdminPresenter.GetMenuItemAsync(id);
			if (menuItem == null)
				return BadRequest();

			return Ok(menuItem);
		}

		[HttpPost]
		public async Task<IActionResult> Up(int id)
		{
			await menuAdminManager.UpAsync(id);
			var menuItems = await menuAdminPresenter.GetMenuItemsAsync();
			menuCache.Initialize();

			return Ok(menuItems);
		}

		[HttpPost]
		public async Task<IActionResult> Down(int id)
		{
			await menuAdminManager.DownAsync(id);
			var menuItems = await menuAdminPresenter.GetMenuItemsAsync();
			menuCache.Initialize();

			return Ok(menuItems);
		}

		[HttpPost]
		public async Task<IActionResult> Update([FromBody] MenuItem menuItem)
		{
			await menuAdminManager.UpdateAsync(menuItem);
			menuCache.Initialize();

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromBody] MenuItem menuItem)
		{
			await menuAdminManager.CreateAsync(menuItem);
			menuCache.Initialize();

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> SetIsHidden(int id, bool isHidden)
		{
			await menuAdminManager.SetIsHiddenAsync(id, isHidden);
			var menuItems = await menuAdminPresenter.GetMenuItemsAsync();
			menuCache.Initialize();

			return Ok(menuItems);
		}

		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			await menuAdminManager.DeleteAsync(id);
			var menuItems = await menuAdminPresenter.GetMenuItemsAsync();
			menuCache.Initialize();

			return Ok(menuItems);
		}
	}
}