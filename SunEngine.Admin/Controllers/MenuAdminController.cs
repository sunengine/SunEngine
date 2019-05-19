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
        
        public async Task<IActionResult> GetMenuItems()
        {
            var menuItems = await menuAdminPresenter.GetMenuItemsAsync();
            return Ok(menuItems);
        }
        
        public async Task<IActionResult> Up(int id)
        {
            await menuAdminManager.UpAsync(id);
            var menuItems = await menuAdminPresenter.GetMenuItemsAsync();
            menuCache.Reset();
            
            return Ok(menuItems);
        }
        
        public async Task<IActionResult> Down(int id)
        {
            await menuAdminManager.DownAsync(id);
            var menuItems = await menuAdminPresenter.GetMenuItemsAsync();
            menuCache.Reset();

            return Ok(menuItems);
        }
        
        public async Task<IActionResult> Update(MenuItem menuItem)
        {
            await menuAdminManager.UpdateAsync(menuItem);
            menuCache.Reset();

            return Ok();
        }
        
        public async Task<IActionResult> Create(MenuItem menuItem)
        {
            await menuAdminManager.CreateAsync(menuItem);
            menuCache.Reset();

            return Ok();
        }
        
        public async Task<IActionResult> SetIsHidden(int menuItemId, bool isHidden)
        {
            await menuAdminManager.SetIsHiddenAsync(menuItemId, isHidden);
            var menuItems = await menuAdminPresenter.GetMenuItemsAsync();
            menuCache.Reset();

            return Ok(menuItems);
        }
    }
}