using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.TextProcess;
using SunEngine.Options;
using SunEngine.Services;
using Flurl;

namespace SunEngine.EntityServices
{
    public class ProfileService : DbService
    {
        private readonly IEmailSender emailSender;
        private readonly Sanitizer sanitizer;
        private readonly GlobalOptions globalOptions;
        
        public ProfileService(
            DataBaseConnection db, 
            IEmailSender emailSender,
            IOptions<GlobalOptions> globalOptions,
            Sanitizer sanitizer
      ) : base(db)
        {
            this.emailSender = emailSender;
            this.sanitizer = sanitizer;
            this.globalOptions = globalOptions.Value;
        }


        public Task<ProfileViewModel> GetProfileAsync(string link)
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

            return query.Select(x =>
                new ProfileViewModel
                {
                    Id = x.Id,
                    Name = x.UserName,
                    Information = x.Information,
                    Link = x.Link,
                    Photo = x.Photo
                }).FirstOrDefaultAsync();
        }

        public Task SendPrivateMessageAsync(User from, User to, string text)
        {
            var header = $"<div>Вам написал: <a href='{globalOptions.SiteUrl.AppendPathSegment("user/"+from.Link)}'>{from.UserName}</a></div><br/>"; 
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
                
            return db.InsertOrReplaceAsync(ban);
        }
        
        public Task UnBunUserAsync(User who, User banned)
        {
            UserBanedUnit ban = new UserBanedUnit
            {
                UserId = who.Id,
                UserBanedId = banned.Id
            };
                
            return db.UserBanedUnits.Where(x=>x.UserId == who.Id && x.UserBanedId == banned.Id).DeleteAsync();
        }
    }

    public class ProfileViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Information { get; set; }
        public string Link { get; set; }

        public string Photo { get; set; }
        //public string Avatar { get; set; }
    }
}