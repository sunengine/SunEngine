using System.Linq;
using System.Threading.Tasks;
using Flurl;
using LinqToDB;
using Microsoft.Extensions.Options;
using SunEngine.Commons.Configuration.Options;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Services;
using SunEngine.Commons.Utils.TextProcess;

namespace SunEngine.Commons.Managers
{
    public interface IProfileManager
    {
        Task SendPrivateMessageAsync(User from, User to, string text);
        Task BanUserAsync(User who, User banned);
        Task UnBanUserAsync(User who, User banned);
    }

    public class ProfileManager : DbService, IProfileManager
    {
        protected readonly IEmailSender emailSender;
        protected readonly Sanitizer sanitizer;
        protected readonly GlobalOptions globalOptions;

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


        public virtual Task SendPrivateMessageAsync(User from, User to, string text)
        {
            var header =
                $"<div>Вам написал: <a href='{globalOptions.SiteUrl.AppendPathSegment("user/" + from.Link)}'>{from.UserName}</a></div><br/>";
            text = sanitizer.Sanitize(header + text);
            string subject = $"Сообщение от {to.UserName} с сайта {globalOptions.SiteName}";

            return emailSender.SendEmailAsync(to.Email, subject, text);
        }

        public virtual Task BanUserAsync(User who, User banned)
        {
            UserBanedUnit ban = new UserBanedUnit
            {
                UserId = who.Id,
                UserBanedId = banned.Id
            };

            return db.InsertAsync(ban);
        }

        public virtual Task UnBanUserAsync(User who, User banned)
        {
            return db.UserBanedUnits.Where(x => x.UserId == who.Id && x.UserBanedId == banned.Id).DeleteAsync();
        }
    }
}