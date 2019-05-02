using System.Linq;
using System.Threading.Tasks;
using Flurl;
using LinqToDB;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;
using SunEngine.Core.Utils.TextProcess;

namespace SunEngine.Core.Managers
{
    public interface IProfileManager
    {
        Task SendPrivateMessageAsync(User from, User to, string text);
        Task BanUserAsync(User who, User banned);
        Task UnBanUserAsync(User who, User banned);
    }

    public class ProfileManager : DbService, IProfileManager
    {
        protected readonly IEmailSenderService EmailSenderService;
        protected readonly Sanitizer sanitizer;
        protected readonly GlobalOptions globalOptions;

        public ProfileManager(
            DataBaseConnection db,
            IEmailSenderService emailSenderService,
            IOptions<GlobalOptions> globalOptions,
            Sanitizer sanitizer
        ) : base(db)
        {
            this.EmailSenderService = emailSenderService;
            this.sanitizer = sanitizer;
            this.globalOptions = globalOptions.Value;
        }


        public virtual Task SendPrivateMessageAsync(User from, User to, string text)
        {
            var header =
                $"<div>Вам написал: <a href='{globalOptions.SiteUrl.AppendPathSegment("user/" + from.Link)}'>{from.UserName}</a></div><br/>";
            text = sanitizer.Sanitize(header + text);
            string subject = $"Сообщение от {to.UserName} с сайта {globalOptions.SiteName}";

            return EmailSenderService.SendEmailAsync(to.Email, subject, text);
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