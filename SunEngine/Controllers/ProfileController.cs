using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Managers;
using SunEngine.Models;
using SunEngine.Presenters;
using SunEngine.Security.Filters;
using SunEngine.Stores;
using SunEngine.Stores.Models;

namespace SunEngine.Controllers
{
    /// <summary>
    /// UserProfile
    /// Контроллер отвечающий за работу страницы /user/id111 профиля пользователей
    /// </summary>
    public class ProfileController : BaseController
    {
        protected readonly ProfileManager profileManager;
        protected readonly IProfilePresenter profilePresenter;
        
        public ProfileController(
            ProfileManager profileManager,
            IProfilePresenter profilePresenter,
            MyUserManager userManager,
            IRolesCache rolesCache) : base(rolesCache, userManager)
        {
            this.profileManager = profileManager;
            this.profilePresenter = profilePresenter;
        }
        
        [HttpPost]
        public virtual async Task<IActionResult> GetProfile(string link)
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
        [Authorize(Roles = RoleStored.UserGroupRegistered)]
        public virtual async Task<IActionResult> SendPrivateMessage(string userId,string text)
        {
            var userTo = await userManager.FindByIdAsync(userId);
            if (userTo == null)
                return BadRequest();

            var userFrom =  await GetUserAsync();
            
            await profileManager.SendPrivateMessageAsync(userFrom, userTo ,text);

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = RoleStored.UserGroupRegistered)]
        public virtual async Task<IActionResult> BanUser(string userId)
        {
            User userBan = await userManager.FindByIdAsync(userId);
            if (userBan == null)
                return BadRequest();
            var roles = await userManager.GetRolesAsync(userBan);
            if (roles.Contains(RoleStored.UserGroupAdmin))
                return BadRequest();

            var user = await GetUserAsync();
            
            await profileManager.BanUserAsync(user,userBan);

            return Ok();
        }
        
        [HttpPost]
        [Authorize(Roles = RoleStored.UserGroupRegistered)]
        public virtual async Task<IActionResult> UnBanUser(string userId)
        {
            User userUnBan = await userManager.FindByIdAsync(userId);
            if (userUnBan == null)
                return BadRequest();
           
            var user = await GetUserAsync();
            
            await profileManager.UnBanUserAsync(user,userUnBan);

            return Ok();
        }
        
        
    }
}

// auth - авторизация, изменить email, пароль
// personal - личная информация (edit self)
// profile - страница профиля (get any user)
