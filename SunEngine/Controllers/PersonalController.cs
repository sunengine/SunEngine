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
        private readonly PersonalService personalService;
        private readonly AuthService authService;

        public PersonalController(
            PersonalService personalService, 
            AuthService authService, 
            MyUserManager userManager,
            IUserGroupStore userGroupStore)
            : base(userGroupStore, userManager)
        {
            this.personalService = personalService;
            this.authService = authService;
        }

        public async Task<MyUserInfoViewModel> GetMyUserInfo()
        {
            return await personalService.GetMyUserInfoAsync(User.UserId);
        }

        public async Task<MyProfileInformationViewModel> GetMyProfileInformation()
        {
            return await personalService.GetMyProfileInformationAsync(User.UserId);
        }

        public async Task SetMyProfileInformation(string html)
        {
            await personalService.SetMyProfileInformationAsync(User.UserId, html);
        }

        public async Task<IActionResult> SetMyLink(string link)
        {
            link = (link+"").Trim();
            
            if (!await personalService.ValidateLinkAsync(User.UserId,link))
            {
                return BadRequest(new ErrorViewModel {ErrorText = "Validation error"});
            }

            await personalService.SetMyLinkAsync(User.UserId, link);

            return Ok(await personalService.GetMyUserInfoAsync(User.UserId));
        }

        public async Task<IActionResult> SetMyName(string password, string name)
        {
            var user = await GetUserAsync();
            if (!await userManager.CheckPasswordAsync(user, password))
            {
                return BadRequest(new ErrorViewModel {ErrorText = "Wrong password"});
            }

            name = Regex.Replace(name.Trim()," {2,}","");

            if (!await personalService.ValidateNameAsync(name,user.Id))
            {
                return BadRequest(new ErrorViewModel {ErrorText = "Validation error"});
            }

            await personalService.SetMyNameAsync(user, name);
            //var token = await authService.GenerateTokenAsync(user);
            //return Ok(new TokenViewModel {Token = token});
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CheckNameInDb(string name)
        {
            return Ok(new {
                yes = await personalService.CheckNameInDbAsync(name,User.UserId)
            });
        }

        [HttpPost]
        public async Task<IActionResult> CheckLinkInDb(string link)
        {
            return Ok(new {
                yes = await personalService.CheckLinkInDbAsync(link,User.UserId)
            });
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveMyAvatar()
        {
            await personalService.RemoveAvatarAsync(User.UserId);
            return Ok();
        }
        
        [HttpPost]
        public async Task<IActionResult> GetMyBanList()
        {
            var usersList = await personalService.GetBanListAsync(User.UserId);

            return Ok(usersList);
        }
    }

    public class MyProfileInformationViewModel
    {
        public string Information { get; set; }
    }

    public class MyUserInfoViewModel
    {
        public string Photo { get; set; }
        public string Avatar { get; set; }
        public string Link { get; set; }
    }
}