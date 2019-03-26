using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.Commons.Utils;
using SunEngine.Commons.Utils.TextProcess;

namespace SunEngine.Commons.Managers
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
    }

    public class PersonalManager : DbService, IPersonalManager
    {
        protected readonly Sanitizer sanitizer;

        public PersonalManager(
            DataBaseConnection db,
            Sanitizer sanitizer) : base(db)
        {
            this.sanitizer = sanitizer;
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
                .Set(x => x.UserName, name).Set(x => x.NormalizedUserName, Normalizer.Singleton.Normalize(name))
                .UpdateAsync();
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
            return db.Users.AnyAsync(x => x.NormalizedUserName == nameNormalized && x.Id != userId);
        }

        public virtual async Task<bool> ValidateNameAsync(string name, int userId)
        {
            var regexAllowedChars = new Regex("^[ а-яА-ЯёЁa-zA-Z0-9-]+$");
            return regexAllowedChars.IsMatch(name) && name.Length >= 3 && !await CheckNameInDbAsync(name, userId);
        }

        public virtual Task RemoveAvatarAsync(int userId)
        {
            return db.Users.Where(x => x.Id == userId)
                .Set(x => x.Photo, User.DefaultAvatar)
                .Set(x => x.Avatar, User.DefaultAvatar).UpdateAsync();
        }
    }
}