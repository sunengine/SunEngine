using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.Models;
using SunEngine.Commons.StoreModels;
using SunEngine.EntityServices;
using SunEngine.Infrastructure;

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
            UserManager<User> userManager) : base(userManager)
        {
            this.profileService = profileService;
        }
        
        [HttpPost]
        public async Task<IActionResult> GetProfile(string link)
        {
            var rez = await profileService.GetProfileAsync(link);
            if (rez == null)
            {
                return NotFound();
            }

            return Json(rez);
        }

        [HttpPost]
        [SpamProtectionFilter(TimeoutSeconds = 60)]
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
    }
}

// auth - авторизация, изменить email, пароль
// personal - личная информация (edit self)
// profile - страница профиля (get any user)
