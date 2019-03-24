using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Services;
using SunEngine.Commons.Controllers;

namespace SunEngine.Admin.Controllers
{
    /// <summary>
    /// Settings roles permissions
    /// </summary>
    public class AdminRolesPermissionsController : AdminBaseController
    {
        private readonly RolesPermissionsAdminService rolesPermissionsAdminService;

        public AdminRolesPermissionsController(
            RolesPermissionsAdminService rolesPermissionsAdminService,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.rolesPermissionsAdminService = rolesPermissionsAdminService;
        }

        [HttpPost]
        public async Task<IActionResult> GetJson()
        {
            var json = await rolesPermissionsAdminService.GetGroupsJsonAsync();

            return Ok(new {Json = json});
        }

        [HttpPost]
        public async Task<IActionResult> UploadJson(string json)
        {
            try
            {
                await rolesPermissionsAdminService.LoadUserGroupsFromJsonAsync(json);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorViewModel
                {
                    ErrorName = e.Message,
                    ErrorText = e.StackTrace
                });
            }

            rolesCache.Reset();

            return Ok();
        }
    }
}