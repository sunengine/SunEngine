using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Managers;
using SunEngine.Admin.Presenters;
using SunEngine.Core.Models;

namespace SunEngine.Admin.Controllers
{
    public class ComponentsAdminController : BaseAdminController
    {
        protected readonly IComponentsAdminPresenter componentsAdminPresenter;
        protected readonly IComponentsAdminManager componentsAdminManager;

        public ComponentsAdminController(
            IComponentsAdminPresenter componentsAdminPresenter,
            IComponentsAdminManager componentsAdminManager,
            IServiceProvider serviceProvider) : base(serviceProvider: serviceProvider)
        {
            this.componentsAdminPresenter = componentsAdminPresenter;
            this.componentsAdminManager = componentsAdminManager;
        }
        
        [HttpPost]
        public async Task<IActionResult> GetAllComponents()
        {
            var rez = await componentsAdminPresenter.GetComponentsAsync();
            return Ok(rez);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddComponent(Component component)
        {
            await componentsAdminManager.CreateComponent(component);
            return Ok();
        }
    }
}
