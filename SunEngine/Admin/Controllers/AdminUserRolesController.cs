using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Presenters;
using SunEngine.Cache;
using SunEngine.Security.Authentication;

namespace SunEngine.Admin.Controllers
{

    public class AdminUserRolesController : AdminBaseController
    {
        private readonly IUserRolesPresenter userRolesPresenter;
        private readonly JwtBlackListService jwtBlackListService;

        public AdminUserRolesController(
            IUserRolesPresenter userRolesPresenter,
            JwtBlackListService jwtBlackListService,
            IRolesCache rolesCache,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.userRolesPresenter = userRolesPresenter;
            this.jwtBlackListService = jwtBlackListService;
        }

        [HttpPost]
        public async Task<IActionResult> GetAllUserRoles()
        {
            var groups = await userRolesPresenter.GetAllRolesAsync();
            return Ok(groups);
        }

        [HttpPost]
        public async Task<IActionResult> GetUserRoles(int userId)
        {
            var groups = await userRolesPresenter.GetUserRolesAsync(userId);
            return Ok(groups);
        }

        [HttpPost]
        public async Task<IActionResult> GetRoleUsers(string roleName, string userNamePart)
        {
            var users = await userRolesPresenter.GetRoleUsers(roleName, userNamePart);
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToRole(int userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            var rez = await userManager.AddToRoleAsync(user, roleName);
            if (!rez.Succeeded) return BadRequest();

            await jwtBlackListService.AddUserTokensAsync(userId);
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromRole(int userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            var rez = await userManager.RemoveFromRoleAsync(user, roleName);
            if (!rez.Succeeded) return BadRequest();

            await jwtBlackListService.AddUserTokensAsync(userId);
            return Ok();
        }
    }
}