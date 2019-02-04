using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Services;
using SunEngine.Commons.Services;
using SunEngine.Controllers;
using SunEngine.Stores;

namespace SunEngine.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GroupsAdminController : BaseController
    {
        private readonly GroupsAdminService groupsAdminService;

        public GroupsAdminController(
            MyUserManager userManager,
            GroupsAdminService groupsAdminService,
            IUserGroupStore userGroupStore) : base(userGroupStore, userManager)
        {
            this.groupsAdminService = groupsAdminService;
        }

        public async Task<IActionResult> GetJson()
        {
            var json = await groupsAdminService.GetGroupsJsonAsync();

            return Ok(new {Json = json});
        }

        public async Task<IActionResult> UploadJson(string json)
        {
            try
            {
                await groupsAdminService.LoadUserGroupsFromJsonAsync(json);
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorViewModel
                {
                    ErrorName = e.Message,
                    ErrorText = e.StackTrace
                });
            }

            userGroupStore.Reset();

            return Ok();
        }
    }
}