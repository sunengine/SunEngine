using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Admin.Services;
using SunEngine.Controllers;
using SunEngine.Managers;
using SunEngine.Stores;

namespace SunEngine.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GroupsUsersController : BaseController
    {
        private readonly GroupsUsersService groupsUsersService;
        
        public GroupsUsersController(
            GroupsUsersService groupsUsersService,
            IUserGroupStore userGroupStore, 
            MyUserManager userManager) : base(userGroupStore, userManager)
        {
            this.groupsUsersService = groupsUsersService;
        }

        public async Task<IActionResult> GetAllUserGroupsAsync()
        {
            return Ok(groupsUsersService.GetAllUserGroupsAsync());
        }
    }
}