using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.Models;
using SunEngine.EntityServices;

namespace SunEngine.Controllers
{
    /// <summary>
    /// UserProfile
    /// Контроллер отвечающий за работу страницы /user/id111 профиля пользователей
    /// </summary>
    public class ProfileController : BaseController
    {
        private readonly UserProfileService userProfileService;
        
        public ProfileController(
            UserProfileService userProfileService,
            UserManager<User> userManager) : base(userManager)
        {
            this.userProfileService = userProfileService;
        }
        
        [HttpPost]
        public async Task<IActionResult> GetProfile(string link)
        {
            var rez = await userProfileService.GetProfileAsync(link);
            if (rez == null)
            {
                return NotFound();
            }

            return Json(rez);
        }
    }
}

// auth - авторизация, изменить email, пароль
// personal - личная информация (edit self)
// profile - страница профиля (get any user)
