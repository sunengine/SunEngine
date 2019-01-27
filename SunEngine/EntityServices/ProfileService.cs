using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.Extensions.Options;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.TextProcess;
using SunEngine.Options;
using SunEngine.Services;
using Flurl;
using SunEngine.Stores;

namespace SunEngine.EntityServices
{
    public class ProfileService : DbService
    {
        private readonly IEmailSender emailSender;
        private readonly Sanitizer sanitizer;
        private readonly GlobalOptions globalOptions;
        private readonly IUserGroupStore userGroupStore;

        public ProfileService(
            DataBaseConnection db,
            IEmailSender emailSender,
            IOptions<GlobalOptions> globalOptions,
            Sanitizer sanitizer,
            IUserGroupStore userGroupStore
        ) : base(db)
        {
            this.emailSender = emailSender;
            this.sanitizer = sanitizer;
            this.globalOptions = globalOptions.Value;
            this.userGroupStore = userGroupStore;
        }


        public async Task<ProfileViewModel> GetProfileAsync(string link, int? viewerUserId)
        {
            IQueryable<User> query;
            if (int.TryParse(link, out int id))
            {
                query = db.Users.Where(x => x.Id == id);
            }
            else
            {
                query = db.Users.Where(x => x.Link == link);
            }

            if (viewerUserId.HasValue)
            {
                int adminGroupId = userGroupStore.AllGroups["Admin"].Id;

                var user = await query.Select(x =>
                    new ProfileViewModel
                    {
                        Id = x.Id,
                        Name = x.UserName,
                        Information = x.Information,
                        Link = x.Link,
                        Photo = x.Photo,
                        NoBunable = x.Roles.Any(y => y.RoleId == adminGroupId),
                        HeBannedMe = x.BanList.Any(y => y.UserBanedId == viewerUserId.Value),
                    }).FirstOrDefaultAsync();

                user.IBannedHim = await db.Users.Where(y => y.Id == viewerUserId.Value)
                    .AnyAsync(x => x.BanList.Any(y => y.UserBanedId == user.Id));

                return user;
            }
            else
            {
                return await query.Select(x =>
                    new ProfileViewModel
                    {
                        Id = x.Id,
                        Name = x.UserName,
                        Information = x.Information,
                        Link = x.Link,
                        Photo = x.Photo,
                        NoBunable = true,
                        HeBannedMe = false,
                        IBannedHim = false
                    }).FirstOrDefaultAsync();
            }
        }

        public Task SendPrivateMessageAsync(User from, User to, string text)
        {
            var header =
                $"<div>Вам написал: <a href='{globalOptions.SiteUrl.AppendPathSegment("user/" + from.Link)}'>{from.UserName}</a></div><br/>";
            text = sanitizer.Sanitize(header + text);
            string subject = $"Сообщение от {to.UserName} с сайта {globalOptions.SiteName}";

            return emailSender.SendEmailAsync(to.Email, subject, text);
        }

        public Task BunUserAsync(User who, User banned)
        {
            UserBanedUnit ban = new UserBanedUnit
            {
                UserId = who.Id,
                UserBanedId = banned.Id
            };

            return db.InsertAsync(ban);
        }

        public Task UnBunUserAsync(User who, User banned)
        {
            UserBanedUnit ban = new UserBanedUnit
            {
                UserId = who.Id,
                UserBanedId = banned.Id
            };

            return db.UserBanedUnits.Where(x => x.UserId == who.Id && x.UserBanedId == banned.Id).DeleteAsync();
        }

        
    }

    public class UserInfoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
    }

    public class ProfileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public string Link { get; set; }

        public string Photo { get; set; }

        public bool NoBunable { get; set; }
        public bool HeBannedMe { get; set; }
        public bool IBannedHim { get; set; }

        //public string Avatar { get; set; }
    }
}