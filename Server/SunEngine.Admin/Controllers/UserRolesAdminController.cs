using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Presenters;
using SunEngine.Core.Cache.Services;
using SunEngine.Core.Security;

namespace SunEngine.Admin.Controllers
{

    public class UserRolesAdminController : BaseAdminController
    {
        private readonly IUserRolesAdminPresenter userRolesAdminPresenter;
        private readonly JweBlackListService jweBlackListService;

        public UserRolesAdminController(
            IUserRolesAdminPresenter userRolesAdminPresenter,
            JweBlackListService jweBlackListService,
            IRolesCache rolesCache,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.userRolesAdminPresenter = userRolesAdminPresenter;
            this.jweBlackListService = jweBlackListService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAllRoles()
        {
            var groups = await userRolesAdminPresenter.GetAllRolesAsync();
            return Ok(groups);
        }

        [HttpPost]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            var groups = await userRolesAdminPresenter.GetUserRolesAsync(userId);
            return Ok(groups);
        }

        [HttpPost]
        public async Task<IActionResult> GetRoleUsers(string roleName, string userNamePart)
        {
            var users = await userRolesAdminPresenter.GetRoleUsers(roleName, userNamePart);
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(int userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            var rez = await userManager.AddToRoleAsync(user, roleName);
            if (!rez.Succeeded) 
                return BadRequest();

            await jweBlackListService.AddAllUserTokensToBlackListAsync(userId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromRole(int userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            var rez = await userManager.RemoveFromRoleAsync(user, roleName);
            if (!rez.Succeeded) return BadRequest();

            await jweBlackListService.AddAllUserTokensToBlackListAsync(userId);
            return Ok();
        }
    }
}
