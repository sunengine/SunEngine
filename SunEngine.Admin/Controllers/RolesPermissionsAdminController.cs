using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Services;

namespace SunEngine.Admin.Controllers
{
    /// <summary>
    /// Settings roles permissions
    /// </summary>
    public class RolesPermissionsAdminController : BaseAdminController
    {
        private readonly RolesPermissionsAdminService rolesPermissionsAdminService;

        public RolesPermissionsAdminController(
            RolesPermissionsAdminService rolesPermissionsAdminService,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.rolesPermissionsAdminService = rolesPermissionsAdminService;
        }

        [HttpPost]
        public async ValueTask<IActionResult> GetJson()
        {
            var json = await rolesPermissionsAdminService.GetRolesJsonAsync();

            return Ok(new {Json = json});
        }

        [HttpPost]
        public async ValueTask<IActionResult> UploadJson(string json)
        {
            await rolesPermissionsAdminService.LoadRolesFromJsonAsync(json);

            rolesCache.Reset();

            return Ok();
        }
    }
}
