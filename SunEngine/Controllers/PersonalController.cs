using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.EntityServices;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
    /// <summary>
    /// Контроллер отвечающий за работу с информацией по текущему пользователю this.User
    /// </summary>
    [Authorize]
    public class PersonalController : BaseController
    {
        private readonly PersonalManager personalManager;
        private readonly AuthService authService;
        private readonly PersonalPresenter personalPresenter;

        public PersonalController(
            PersonalManager personalManager, 
            AuthService authService, 
            PersonalPresenter personalPresenter,
            MyUserManager userManager,
            IUserGroupStore userGroupStore)
            : base(userGroupStore, userManager)
        {
            this.personalManager = personalManager;
            this.authService = authService;
            this.personalPresenter = personalPresenter;
        }

        public async Task<MyUserInfoViewModel> GetMyUserInfo()
        {
            return await personalPresenter.GetMyUserInfoAsync(User.UserId);
        }

        public async Task<MyProfileInformationViewModel> GetMyProfileInformation()
        {
            return await personalPresenter.GetMyProfileInformationAsync(User.UserId);
        }

        public async Task SetMyProfileInformation(string html)
        {
            await personalManager.SetMyProfileInformationAsync(User.UserId, html);
        }

        public async Task<IActionResult> SetMyLink(string link)
        {
            link = (link+"").Trim();
            
            if (!await personalManager.ValidateLinkAsync(User.UserId,link))
            {
                return BadRequest(new ErrorViewModel {ErrorText = "Validation error"});
            }

            await personalManager.SetMyLinkAsync(User.UserId, link);

            return Ok(await personalPresenter.GetMyUserInfoAsync(User.UserId));
        }

        public async Task<IActionResult> SetMyName(string password, string name)
        {
            var user = await GetUserAsync();
            if (!await userManager.CheckPasswordAsync(user, password))
            {
                return BadRequest(new ErrorViewModel {ErrorText = "Wrong password"});
            }

            name = Regex.Replace(name.Trim()," {2,}","");

            if (!await personalManager.ValidateNameAsync(name,user.Id))
            {
                return BadRequest(new ErrorViewModel {ErrorText = "Validation error"});
            }

            await personalManager.SetMyNameAsync(user, name);
            //var token = await authService.GenerateTokenAsync(user);
            //return Ok(new TokenViewModel {Token = token});  TODO
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CheckNameInDb(string name)
        {
            return Ok(new {
                yes = await personalManager.CheckNameInDbAsync(name,User.UserId)
            });
        }

        [HttpPost]
        public async Task<IActionResult> CheckLinkInDb(string link)
        {
            return Ok(new {
                yes = await personalManager.CheckLinkInDbAsync(link,User.UserId)
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveMyAvatar()
        {
            await personalManager.RemoveAvatarAsync(User.UserId);
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> GetMyBanList()
        {
            var usersList = await personalPresenter.GetBanListAsync(User.UserId);

            return Ok(usersList);
        }
    }

    
}