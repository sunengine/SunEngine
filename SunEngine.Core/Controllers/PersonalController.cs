using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.Errors;
using SunEngine.Core.Managers;
using SunEngine.Core.Presenters;
using SunEngine.Core.Security;

namespace SunEngine.Core.Controllers
{
    /// <summary>
    /// User info get set controller for this.User
    /// </summary>
    [Authorize]
    public class PersonalController : BaseController
    {
        protected readonly IPersonalManager personalManager;
        protected readonly JweService jweService;
        protected readonly IPersonalPresenter personalPresenter;

        public PersonalController(
            IPersonalManager personalManager, 
            JweService jweService, 
            IPersonalPresenter personalPresenter,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.personalManager = personalManager;
            this.jweService = jweService;
            this.personalPresenter = personalPresenter;

        }

        [HttpPost]
        public virtual async Task<IActionResult> GetMyUserInfo()
        {
            return Ok(await personalPresenter.GetMyUserInfoAsync(User.UserId));
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetMyProfileInformation()
        {
            return Ok(await personalPresenter.GetMyProfileInformationAsync(User.UserId));
        }

        [HttpPost]
        public virtual async Task SetMyProfileInformation(string html)
        {
            await personalManager.SetMyProfileInformationAsync(User.UserId, html);
        }

        [HttpPost]
        public virtual async Task<IActionResult> SetMyLink(string link)
        {
            link = (link+"").Trim();
            
            if (!await personalManager.ValidateLinkAsync(User.UserId,link))
                return BadRequest(new ErrorView ("LinkInvalid","Link validation error", ErrorType.System));

            await personalManager.SetMyLinkAsync(User.UserId, link);

            return Ok(await personalPresenter.GetMyUserInfoAsync(User.UserId));
        }

        [HttpPost]
        public virtual async Task<IActionResult> SetMyName(string password, string name)
        {
            var user = await GetUserAsync();
            if (!await userManager.CheckPasswordAsync(user, password))
                return BadRequest(new ErrorView ("PasswordInvalid","Wrong password", ErrorType.System));

            name = Regex.Replace(name.Trim()," {2,}","");

            if (!await personalManager.ValidateNameAsync(name,user.Id))
                return BadRequest(new ErrorView ("NameInvalid","Validation error", ErrorType.System));

            await personalManager.SetMyNameAsync(user, name);

            Response.Headers.Clear(); 
            
            await jweService.RenewSecurityTokensAsync(HttpContext, user, User.SessionId);

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
        
        [HttpPost]
        public virtual async Task<IActionResult> GetMySessions()
        {
           
            var sessions = await personalPresenter.GetMySessionsAsync(User.UserId, User.SessionId);

            return Ok(sessions);
        }
        
        [HttpPost]
        public virtual async Task<IActionResult> RemoveMySessions(string sessions)
        {
            long[] sessionsIds = sessions.Split(',').Select(long.Parse).ToArray(); 
            
            await personalManager.RemoveSessionsAsync(User.UserId, sessionsIds);

            return Ok();
        }
    }
}
