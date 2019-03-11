using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Options;
using SunEngine.Configuration.Options;
using SunEngine.DataBase;
using SunEngine.Models;
using SunEngine.Services;
using SunEngine.Utils;
using SunEngine.Utils.TextProcess;

namespace SunEngine.Managers
{
    public interface IPersonalManager
    {
        Task SetPhotoAsync(int userId, string photo);
        Task SetAvatarAsync(int userId, string avatar);
        Task SetPhotoAndAvatarAsync(int userId, string photo, string avatar);
        Task SetMyProfileInformationAsync(int userId, string html);
        Task SetMyLinkAsync(int userId, string link);
        Task SetMyNameAsync(User user, string name);
        Task<bool> CheckLinkInDbAsync(string link, int userId);
        Task<bool> ValidateLinkAsync(int userId, string link);
        Task<bool> CheckNameInDbAsync(string name, int userId);
        Task<bool> ValidateNameAsync(string name, int userId);
        Task RemoveAvatarAsync(int userId);
        Task SendChangeEmailConfirmationMessageByEmailAsync(User user, string email);
    }

    public class PersonalManager : DbService, IPersonalManager
    {
        protected readonly Sanitizer sanitizer;
        protected readonly IUrlHelperFactory urlHelperFactory;
        protected readonly IActionContextAccessor accessor;
        protected readonly MyUserManager userManager;
        protected readonly GlobalOptions globalOptions;
        protected readonly IEmailSender emailSender;

        
        public PersonalManager(
            MyUserManager userManager,
            IEmailSender emailSender,
            DataBaseConnection db, 
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor accessor,
            IOptions<GlobalOptions> globalOptions,
            Sanitizer sanitizer) : base(db)
        {
            this.sanitizer = sanitizer;
            this.urlHelperFactory = urlHelperFactory;
            this.accessor = accessor;
            this.globalOptions = globalOptions.Value;
            this.userManager = userManager;
            this.emailSender = emailSender;
        }

        public virtual Task SetPhotoAsync(int userId, string photo)
        {
            return db.Users.Where(x => x.Id == userId).Set(x => x.Photo, x => photo).UpdateAsync();
        }

        public virtual Task SetAvatarAsync(int userId, string avatar)
        {
            return db.Users.Where(x => x.Id == userId).Set(x => x.Avatar, x => avatar).UpdateAsync();
        }

        public virtual Task SetPhotoAndAvatarAsync(int userId, string photo, string avatar)
        {
            return db.Users.Where(x => x.Id == userId)
                .Set(x => x.Photo, x => photo)
                .Set(x => x.Avatar, x => avatar)
                .UpdateAsync();
        }

        public virtual Task SetMyProfileInformationAsync(int userId, string html)
        {
            var htmlSanitized = sanitizer.Sanitize(html);
            return db.Users.Where(x => x.Id == userId)
                .Set(x => x.Information, htmlSanitized).UpdateAsync();
        }

        public virtual Task SetMyLinkAsync(int userId, string link)
        {
            if (string.IsNullOrEmpty(link))
            {
                return db.Users.Where(x => x.Id == userId)
                    .Set(x => x.Link, x => x.Id.ToString()).UpdateAsync();
            }

            return db.Users.Where(x => x.Id == userId)
                .Set(x => x.Link, link).UpdateAsync();
        }

        public virtual Task SetMyNameAsync(User user, string name)
        {
            user.UserName = name;
            return db.Users.Where(x => x.Id == user.Id)
                .Set(x => x.UserName, name).Set(x => x.NormalizedUserName, Normalizer.Singleton.Normalize(name)).UpdateAsync();
        }

        public virtual Task<bool> CheckLinkInDbAsync(string link, int userId)
        {
            return db.Users.AnyAsync(x => x.Link.ToLower() == link.ToLower() && x.Id != userId);
        }

        public virtual async Task<bool> ValidateLinkAsync(int userId, string link)
        {
            if (string.IsNullOrEmpty(link))
                return true;

            bool allowId = link == userId.ToString();
            bool allowedChars = Regex.IsMatch(link, "^[a-zA-Z0-9-]+$");
            bool needChar = Regex.IsMatch(link, "[a-zA-Z]");
            bool allowedLength = link.Length >= 3;
            bool alreadyInDb = await CheckLinkInDbAsync(link, userId);
            if (allowId)
                return true;
            return allowedChars && needChar && allowedLength && !alreadyInDb;
        }

        public virtual Task<bool> CheckNameInDbAsync(string name, int userId)
        {
            var nameNormalized = Normalizer.Normalize(name);
            return db.Users.AnyAsync(x => x.NormalizedUserName == nameNormalized && x.Id != userId );
        }
        
        public virtual async Task<bool> ValidateNameAsync(string name, int userId)
        {
            var regexAllowedChars = new Regex("^[ а-яА-ЯёЁa-zA-Z0-9-]+$");
            return regexAllowedChars.IsMatch(name) && name.Length >= 3 && !await CheckNameInDbAsync(name,userId);
        }

        public virtual Task RemoveAvatarAsync(int userId)
        {
            return db.Users.Where(x => x.Id == userId)
                .Set(x => x.Photo, User.DefaultAvatar)
                .Set(x => x.Avatar, User.DefaultAvatar).UpdateAsync();
        }
        
        public virtual async Task SendChangeEmailConfirmationMessageByEmailAsync(User user, string email)
        {
            var urlHelper = GetUrlHelper();

            var emailToken = await userManager.GenerateChangeEmailTokenAsync(user, email);

            var (schema, host) = globalOptions.GetSchemaAndHostApi();

            var updateEmailUrl = urlHelper.Action("ConfirmEmail", "Account",
                new {token = emailToken}, schema, host);

            await emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Confirm your email by clicking this <a href=\"{updateEmailUrl}\">link</a>.");
        }

        
        protected IUrlHelper GetUrlHelper()
        {
            ActionContext context = accessor.ActionContext;
            return urlHelperFactory.GetUrlHelper(context);
        }
    }
}