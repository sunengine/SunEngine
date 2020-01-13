using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using LinqToDB;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;
using SunEngine.Core.Services;

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
		protected readonly IEmailSenderService emailSenderService;
		protected readonly SanitizerService sanitizerService;
		protected readonly IOptionsMonitor<GlobalOptions> globalOptions;

		public ProfileManager(
			DataBaseConnection db,
			IEmailSenderService emailSenderService,
			IOptionsMonitor<GlobalOptions> globalOptions,
			SanitizerService sanitizerService
		) : base(db)
		{
			this.emailSenderService = emailSenderService;
			this.sanitizerService = sanitizerService;
			this.globalOptions = globalOptions;
		}


		public virtual Task SendPrivateMessageAsync(User from, User to, string text)
		{
			return emailSenderService.SendEmailByTemplateAsync(
				to.Email,
				"private-message.html",
				new Dictionary<string, string>
				{
					{"[siteName]", globalOptions.CurrentValue.SiteName},
					{
						"[url]",
						globalOptions.CurrentValue.SiteUrl.AppendPathSegment(
							"user/" + (from.Link ?? from.Id.ToString()))
					},
					{"[userName]", from.UserName},
					{"[message]", sanitizerService.Sanitize(text)}
				}
			);
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