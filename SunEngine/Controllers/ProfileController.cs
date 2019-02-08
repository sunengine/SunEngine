using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.Commons.StoreModels;
using SunEngine.Filters;
using SunEngine.Managers;
using SunEngine.Presenters;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
    /// <summary>
    /// UserProfile
    /// Контроллер отвечающий за работу страницы /user/id111 профиля пользователей
    /// </summary>
    public class ProfileController : BaseController
    {
        private readonly ProfileManager profileManager;
        private readonly ProfilePresenter profilePresenter;
        
        public ProfileController(
            ProfileManager profileManager,
            ProfilePresenter profilePresenter,
            MyUserManager userManager,
            IUserGroupStore userGroupStore) : base(userGroupStore, userManager)
        {
            this.profileManager = profileManager;
            this.profilePresenter = profilePresenter;
        }
        
        [HttpPost]
        public async Task<IActionResult> GetProfile(string link)
        {
            int? userId = User?.UserId;
            
            
            var rez = await profilePresenter.GetProfileAsync(link, userId);
            if (rez == null)
            {
                return NotFound();
            }

            return Json(rez);
        }

        [HttpPost]
        [UserSpamProtectionFilter(TimeoutSeconds = 60)]
        [Authorize(Roles = UserGroup.UserGroupRegistered)]
        public async Task<IActionResult> SendPrivateMessage(string userId,string text)
        {
            var userTo = await userManager.FindByIdAsync(userId);
            if (userTo == null)
                return BadRequest();

            var userFrom =  await GetUserAsync();
            
            await profileManager.SendPrivateMessageAsync(userFrom, userTo ,text);

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
            
            await profileManager.BanUserAsync(user,userBan);

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
            
            await profileManager.UnBanUserAsync(user,userUnBan);

            return Ok();
        }
        
        
    }
}

// auth - авторизация, изменить email, пароль
// personal - личная информация (edit self)
// profile - страница профиля (get any user)
