using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LinqToDB;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.Commons.TextProcess;
using SunEngine.Services;

namespace SunEngine.Managers
{
    public class PersonalManager : DbService
    {
        private readonly Sanitizer sanitizer;
        
        public PersonalManager(
            DataBaseConnection db, 
            Sanitizer sanitizer) : base(db)
        {
            this.sanitizer = sanitizer;
        }

        public Task SetPhotoAsync(int id, string photo)
        {
            return db.Users.Where(x => x.Id == id).Set(x => x.Photo, x => photo).UpdateAsync();
        }

        public Task SetAvatarAsync(int id, string avatar)
        {
            return db.Users.Where(x => x.Id == id).Set(x => x.Avatar, x => avatar).UpdateAsync();
        }

        public Task SetPhotoAndAvatarAsync(int id, string photo, string avatar)
        {
            return db.Users.Where(x => x.Id == id)
                .Set(x => x.Photo, x => photo)
                .Set(x => x.Avatar, x => avatar)
                .UpdateAsync();
        }

        

        public Task SetMyProfileInformationAsync(int id, string html)
        {
            var htmlSanitized = sanitizer.Sanitize(html);
            return db.Users.Where(x => x.Id == id)
                .Set(x => x.Information, htmlSanitized).UpdateAsync();
        }

        public Task SetMyLinkAsync(int id, string link)
        {
            if (string.IsNullOrEmpty(link))
            {
                return db.Users.Where(x => x.Id == id)
                    .Set(x => x.Link, x => x.Id.ToString()).UpdateAsync();
            }
            else
            {
                return db.Users.Where(x => x.Id == id)
                    .Set(x => x.Link, link).UpdateAsync();
            }
        }

        public Task SetMyNameAsync(User user, string name)
        {
            user.UserName = name;
            return db.Users.Where(x => x.Id == user.Id)
                .Set(x => x.UserName, name).Set(x => x.NormalizedUserName, Normalizer.Singleton.Normalize(name)).UpdateAsync();
        }

        public Task<bool> CheckLinkInDbAsync(string link, int userId)
        {
            return db.Users.AnyAsync(x => x.Link.ToLower() == link.ToLower() && x.Id != userId);
        }

        public async Task<bool> ValidateLinkAsync(int userId, string link)
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

        public Task<bool> CheckNameInDbAsync(string name, int userId)
        {
            return db.Users.AnyAsync(x => x.NormalizedUserName == name.ToUpper() && x.Id != userId );
        }
        
        
        public async Task<bool> ValidateNameAsync(string name, int userId)
        {
            var regexAllowedChars = new Regex("^[ а-яА-ЯёЁa-zA-Z0-9-]+$");
            return regexAllowedChars.IsMatch(name) && name.Length >= 3 && !await CheckNameInDbAsync(name,userId);
        }

        public Task RemoveAvatarAsync(int userId)
        {
            return db.Users.Where(x => x.Id == userId)
                .Set(x => x.Photo, User.DefaultAvatar)
                .Set(x => x.Avatar, User.DefaultAvatar).UpdateAsync();
        }
        
        
    }
}