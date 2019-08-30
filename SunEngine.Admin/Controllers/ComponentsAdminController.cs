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
        public async Task<IActionResult> GetComponent(string name)
        {
            var component = await componentsAdminPresenter.GetComponentAsync(name);
            return Ok(component);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddComponent([FromBody]Component component)
        {
            await componentsAdminManager.CreateComponentAsync(component);
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateComponent([FromBody]Component component)
        {
            await componentsAdminManager.CreateComponentAsync(component);
            return Ok();
        }
    }
}
