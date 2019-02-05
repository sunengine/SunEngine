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
    public class ProfileManager : DbService
    {
        private readonly IEmailSender emailSender;
        private readonly Sanitizer sanitizer;
        private readonly GlobalOptions globalOptions;

        public ProfileManager(
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


        

        public Task SendPrivateMessageAsync(User from, User to, string text)
        {
            var header =
                $"<div>Вам написал: <a href='{globalOptions.SiteUrl.AppendPathSegment("user/" + from.Link)}'>{from.UserName}</a></div><br/>";
            text = sanitizer.Sanitize(header + text);
            string subject = $"Сообщение от {to.UserName} с сайта {globalOptions.SiteName}";

            return emailSender.SendEmailAsync(to.Email, subject, text);
        }

        public Task BanUserAsync(User who, User banned)
        {
            UserBanedUnit ban = new UserBanedUnit
            {
                UserId = who.Id,
                UserBanedId = banned.Id
            };

            return db.InsertAsync(ban);
        }

        public Task UnBanUserAsync(User who, User banned)
        {
            UserBanedUnit ban = new UserBanedUnit
            {
                UserId = who.Id,
                UserBanedId = banned.Id
            };

            return db.UserBanedUnits.Where(x => x.UserId == who.Id && x.UserBanedId == banned.Id).DeleteAsync();
        }

        
    }

    
}