using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Presenters;

namespace SunEngine.Admin.Controllers
{
    public class MenuAdminController : BaseAdminController
    {
        protected readonly IMenuAdminManager menuAdminManager;
        protected readonly IMenuAdminPresenter menuAdminPresenter;
        
        public MenuAdminController(
            IMenuAdminManager menuAdminManager,
            IMenuAdminPresenter menuAdminPresenter,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.menuAdminManager = menuAdminManager;
            this.menuAdminPresenter = menuAdminPresenter;
        }
        
        public async Task<IActionResult> GetMenuItems()
        {
            var menuItems = await menuAdminPresenter.GetMenuItemsAsync();
            return Ok(menuItems);
        }
    }
}