using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SunEngine.Core.Cache.Services.Counters;
using SunEngine.Core.Filters;
using SunEngine.Core.Managers;
using SunEngine.Core.Models;
using SunEngine.Core.Presenters;
using SunEngine.Core.Security;

namespace SunEngine.Core.Controllers
{
    /// <summary>
    /// User profile controller for "/user/userLink" 
    /// Get profile, send private message, (un)ban user
    /// </summary>
    public class ProfileController : BaseController
    {
        protected readonly IProfileManager profileManager;
        protected readonly IProfilePresenter profilePresenter;
        protected readonly IProfilesVisitsCounterService profilesVisitsCounterService;

        public ProfileController(
            IProfileManager profileManager,
            IProfilePresenter profilePresenter,
            IProfilesVisitsCounterService profilesVisitsCounterService,
            IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.profileManager = profileManager;
            this.profilePresenter = profilePresenter;
            this.profilesVisitsCounterService = profilesVisitsCounterService;
        }

        [HttpPost]
        public virtual async Task<IActionResult> GetProfile(string link)
        {
            int? userId = User?.UserId;

            ProfileView profileView = await profilePresenter.GetProfileAsync(link, userId);
            if (profileView == null)
                return NotFound();

            profileView.ProfileVisitsCount += profilesVisitsCounterService.CountProfile(UserOrIpKey, profileView.Id);

            return Json(profileView);
        }

        [HttpPost]
        [UserSpamProtectionFilter(TimeoutSeconds = 60)]
        [Authorize(Roles = RoleNames.Registered)]
        public virtual async Task<IActionResult> SendPrivateMessage(string userId, string text)
        {
            var userTo = await userManager.FindByIdAsync(userId);
            if (userTo == null)
                return BadRequest();

            var userFrom = await GetUserAsync();

            await profileManager.SendPrivateMessageAsync(userFrom, userTo, text);

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.Registered)]
        public virtual async Task<IActionResult> BanUser(string userId)
        {
            User userBan = await userManager.FindByIdAsync(userId);
            if (userBan == null)
                return BadRequest();
            var roles = await userManager.GetRolesAsync(userBan);
            if (roles.Contains(RoleNames.Admin))
                return BadRequest();

            var user = await GetUserAsync();

            await profileManager.BanUserAsync(user, userBan);

            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = RoleNames.Registered)]
        public virtual async Task<IActionResult> UnBanUser(string userId)
        {
            User userUnBan = await userManager.FindByIdAsync(userId);
            if (userUnBan == null)
                return BadRequest();

            var user = await GetUserAsync();

            await profileManager.UnBanUserAsync(user, userUnBan);

            return Ok();
        }
    }
}

// auth - авторизация, изменить email, пароль
// personal - личная информация (edit self)
// profile - страница профиля (get any user)
