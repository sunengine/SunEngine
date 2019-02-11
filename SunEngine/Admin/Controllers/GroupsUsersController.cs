using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Services;
using SunEngine.Controllers;
using SunEngine.Managers;
using SunEngine.Security.Authentication;
using SunEngine.Stores;

namespace SunEngine.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GroupsUsersController : BaseController
    {
        private readonly GroupsUsersService groupsUsersService;
        private readonly JwtBlackListService jwtBlackListService;

        public GroupsUsersController(
            GroupsUsersService groupsUsersService,
            JwtBlackListService jwtBlackListService,
            IUserGroupStore userGroupStore,
            MyUserManager userManager) : base(userGroupStore, userManager)
        {
            this.groupsUsersService = groupsUsersService;
            this.jwtBlackListService = jwtBlackListService;
        }

        public async Task<IActionResult> GetAllUserGroupsAsync()
        {
            var groups = await groupsUsersService.GetAllUserGroupsAsync();
            return Ok(groups);
        }

        public async Task<IActionResult> GetUserGroupsAsync(int userId)
        {
            var groups = await groupsUsersService.GetUserGroupsAsync(userId);
            return Ok(groups);
        }

        public async Task<IActionResult> AddUserToRoleAsync(int userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            var rez = await userManager.AddToRoleAsync(user, roleName);
            if (!rez.Succeeded) return BadRequest();
            
            await  jwtBlackListService.AddUserTokensAsync(userId);
            return Ok();

        }

        public async Task<IActionResult> RemoveUserFromRoleAsync(int userId, string roleName)
        {
            var user = await userManager.FindByIdAsync(userId);
            var rez = await userManager.RemoveFromRoleAsync(user, roleName);
            if (!rez.Succeeded) return BadRequest();
            
            await jwtBlackListService.AddUserTokensAsync(userId);
            return Ok();

        }
    }
}