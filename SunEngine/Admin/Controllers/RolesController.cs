using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Services;
using SunEngine.Controllers;
using SunEngine.Managers;
using SunEngine.Security;
using SunEngine.Stores;

namespace SunEngine.Admin.Controllers
{
    public class RolesController : AdminBaseController
    {
        private readonly RolesAdminService rolesAdminService;

        public RolesController(
            MyUserManager userManager,
            RolesAdminService rolesAdminService,
            IRolesCache rolesCache) : base(rolesCache, userManager)
        {
            this.rolesAdminService = rolesAdminService;
        }

        public async Task<IActionResult> GetJson()
        {
            var json = await rolesAdminService.GetGroupsJsonAsync();

            return Ok(new {Json = json});
        }

        public async Task<IActionResult> UploadJson(string json)
        {
            try
            {
                await rolesAdminService.LoadUserGroupsFromJsonAsync(json);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorViewModel
                {
                    ErrorName = e.Message,
                    ErrorText = e.StackTrace
                });
            }

            RolesCache.Reset();

            return Ok();
        }
    }
}