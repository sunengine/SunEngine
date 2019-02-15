using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Managers;
using SunEngine.Presenters;
using SunEngine.Security.Authorization;
using SunEngine.Stores;

namespace SunEngine.Controllers
{
    /// <summary>
    /// Контроллер отвечающий за работу с информацией по текущему пользователю this.User
    /// </summary>
    [Authorize]
    public class PersonalController : BaseController
    {
        protected readonly PersonalManager personalManager;
        protected readonly JwtService jwtService;
        protected readonly IPersonalPresenter personalPresenter;

        public PersonalController(
            PersonalManager personalManager, 
            JwtService jwtService, 
            IPersonalPresenter personalPresenter,
            MyUserManager userManager,
            IRolesCache rolesCache)
            : base(rolesCache, userManager)
        {
            this.personalManager = personalManager;
            this.jwtService = jwtService;
            this.personalPresenter = personalPresenter;
        }

        public virtual async Task<IActionResult> GetMyUserInfo()
        {
            return Ok(await personalPresenter.GetMyUserInfoAsync(User.UserId));
        }

        public virtual async Task<IActionResult> GetMyProfileInformation()
        {
            return Ok(await personalPresenter.GetMyProfileInformationAsync(User.UserId));
        }

        public virtual async Task SetMyProfileInformation(string html)
        {
            await personalManager.SetMyProfileInformationAsync(User.UserId, html);
        }

        public virtual async Task<IActionResult> SetMyLink(string link)
        {
            link = (link+"").Trim();
            
            if (!await personalManager.ValidateLinkAsync(User.UserId,link))
            {
                return BadRequest(new ErrorViewModel {ErrorText = "Validation error"});
            }

            await personalManager.SetMyLinkAsync(User.UserId, link);

            return Ok(await personalPresenter.GetMyUserInfoAsync(User.UserId));
        }

        public virtual async Task<IActionResult> SetMyName(string password, string name)
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

            Response.Headers.Clear(); 
            
            await jwtService.RenewSecurityTokensAsync(Response, user, User.SessionId);

            return Ok();
        }

        [HttpPost]
        public virtual async Task<IActionResult> CheckNameInDb(string name)
        {
            return Ok(new {
                yes = await personalManager.CheckNameInDbAsync(name,User.UserId)
            });
        }

        [HttpPost]
        public virtual async Task<IActionResult> CheckLinkInDb(string link)
        {
            return Ok(new {
                yes = await personalManager.CheckLinkInDbAsync(link,User.UserId)
            });
        }
        
        [HttpPost]
        public virtual async Task<IActionResult> RemoveMyAvatar()
        {
            await personalManager.RemoveAvatarAsync(User.UserId);
            return Ok();
        }
        
        [HttpPost]
        public virtual async Task<IActionResult> GetMyBanList()
        {
            var usersList = await personalPresenter.GetBanListAsync(User.UserId);

            return Ok(usersList);
        }
    }

    
}