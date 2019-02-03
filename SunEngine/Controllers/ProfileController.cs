using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.Commons.StoreModels;
using SunEngine.EntityServices;
using SunEngine.Infrastructure;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
    /// <summary>
    /// UserProfile
    /// Контроллер отвечающий за работу страницы /user/id111 профиля пользователей
    /// </summary>
    public class ProfileController : BaseController
    {
        private readonly ProfileService profileService;
        
        public ProfileController(
            ProfileService profileService,
            MyUserManager userManager,
            IUserGroupStore userGroupStore) : base(userGroupStore, userManager)
        {
            this.profileService = profileService;
        }
        
        [HttpPost]
        public async Task<IActionResult> GetProfile(string link)
        {
            int? userId = User?.UserId;
            
            
            var rez = await profileService.GetProfileAsync(link, userId);
            if (rez == null)
            {
                return NotFound();
            }

            return Json(rez);
        }

        [HttpPost]
        [SpamProtectionFilterUser(TimeoutSeconds = 60)]
        [Authorize(Roles = UserGroup.UserGroupRegistered)]
        public async Task<IActionResult> SendPrivateMessage(string userId,string text)
        {
            var userTo = await userManager.FindByIdAsync(userId);
            if (userTo == null)
                return BadRequest();

            var userFrom =  await GetUserAsync();
            
            await profileService.SendPrivateMessageAsync(userFrom, userTo ,text);

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = UserGroup.UserGroupRegistered)]
        public async Task<IActionResult> BanUser(string userId)
        {
            User userBan = await userManager.FindByIdAsync(userId);
            if (userBan == null)
                return BadRequest();
            var roles = await userManager.GetRolesAsync(userBan);
            if (roles.Contains(UserGroup.UserGroupAdmin))
                return BadRequest();

            var user = await GetUserAsync();
            
            await profileService.BunUserAsync(user,userBan);

            return Ok();
        }
        
        [HttpPost]
        [Authorize(Roles = UserGroup.UserGroupRegistered)]
        public async Task<IActionResult> UnBanUser(string userId)
        {
            User userUnBan = await userManager.FindByIdAsync(userId);
            if (userUnBan == null)
                return BadRequest();
           
            var user = await this.GetUserAsync();
            
            await profileService.UnBunUserAsync(user,userUnBan);

            return Ok();
        }
        
        
    }
}

// auth - авторизация, изменить email, пароль
// personal - личная информация (edit self)
// profile - страница профиля (get any user)
