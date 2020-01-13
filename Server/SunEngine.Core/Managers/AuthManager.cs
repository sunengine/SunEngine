using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using LinqToDB;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.Controllers;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors;
using SunEngine.Core.Errors.Exceptions;
using SunEngine.Core.Models;
using SunEngine.Core.Security;
using SunEngine.Core.Services;
using SunEngine.Core.Utils;

namespace SunEngine.Core.Managers
{
	public interface IAuthManager
	{
		Task<User> LoginAsync(string nameOrEmail, string password);
		Task LogoutAsync(int userId, long sessionId);
		Task RegisterAsync(NewUserArgs model);
		Task<bool> CheckUserNameInDbAsync(string userName);
	}


	public class AuthManager : DbService, IAuthManager
	{
		protected readonly SunUserManager userManager;
		protected readonly IOptionsMonitor<GlobalOptions> globalOptions;
		protected readonly IEmailSenderService emailSenderService;
		protected readonly ILogger logger;
		protected readonly JweBlackListService jweBlackListService;


		public AuthManager(
			SunUserManager userManager,
			IEmailSenderService emailSenderService,
			DataBaseConnection db,
			IOptionsMonitor<GlobalOptions> globalOptions,
			JweBlackListService jweBlackListService,
			ILoggerFactory loggerFactory) : base(db)
		{
			this.userManager = userManager;
			this.globalOptions = globalOptions;
			this.emailSenderService = emailSenderService;
			this.jweBlackListService = jweBlackListService;
			logger = loggerFactory.CreateLogger<AccountController>();
		}


		public async Task<User> LoginAsync(string nameOrEmail, string password)
		{
			User user = await userManager.FindUserByNameOrEmailAsync(nameOrEmail);

			if (user == null || !await userManager.CheckPasswordAsync(user, password))
				throw new SunErrorException(new Error("UsernamePasswordInvalid", "The username or password is invalid.",
					ErrorType.Soft));

			if (!await userManager.IsEmailConfirmedAsync(user))
				throw new SunErrorException(new Error("EmailNotConfirmed", "You must have a confirmed email to log in.",
					ErrorType.Soft));

			if (await userManager.IsUserInRoleAsync(user.Id, RoleNames.Banned))
				throw new SunErrorException(new Error("UserBanned", "User is banned", ErrorType.System));

			return user;
		}

		public virtual async Task LogoutAsync(int userId, long sessionId)
		{
			await jweBlackListService.AddUserTokensToBlackListAsync(userId, new[] {sessionId});
			await db.LongSessions.Where(x => x.UserId == userId && x.Id == sessionId).DeleteAsync();
		}

		public virtual async Task RegisterAsync(NewUserArgs model)
		{
			var user = new User
			{
				UserName = model.UserName,
				Email = model.Email,
				RegisteredDate = DateTime.UtcNow
			};

			using (db.BeginTransaction())
			{
				IdentityResult result = await userManager.CreateAsync(user, model.Password);

				if (!result.Succeeded)
				{
					var error = result.Errors.FirstOrDefault();

					if (result.Errors.All(x => x.Code != "DuplicateEmail"))
						throw new SunErrorException(new Error(error.Code, error.Description, ErrorType.System));

					user = await userManager.FindByEmailAsync(model.Email);
					if (user.EmailConfirmed)
						throw new SunErrorException(new Error(error.Code, error.Description, ErrorType.System));

					user.UserName = model.UserName;
					user.PasswordHash = userManager.PasswordHasher.HashPassword(user, model.Password);

					result = await userManager.UpdateAsync(user);

					if (!result.Succeeded)
						throw new SunErrorException(new Error(error.Code, error.Description, ErrorType.System));
				}
				else
				{
					await db.Users.Where(x => x.Id == user.Id).Set(x => x.Link, x => x.Id.ToString())
						.UpdateAsync();

					logger.LogInformation($"New user registered (id: {user.Id})");
				}

				// Send email confirmation email
				var confirmToken = await userManager.GenerateEmailConfirmationTokenAsync(user);

				var emailConfirmUrl = globalOptions.CurrentValue.SiteApi
					.AppendPathSegments("Auth", "ConfirmRegister")
					.SetQueryParams(new {uid = user.Id, token = confirmToken});

				try
				{
					await emailSenderService.SendEmailByTemplateAsync(
						model.Email,
						"register.html",
						new Dictionary<string, string> {{"[link]", emailConfirmUrl}}
					);
				}
				catch (Exception exception)
				{
					throw new SunErrorException(new Error("EmailSendError", "Can not send email",
						ErrorType.System, exception));
				}


				logger.LogInformation($"Sent email confirmation email (id: {user.Id})");

				db.CommitTransaction();
			}
		}

		public Task<bool> CheckUserNameInDbAsync(string userName)
		{
			var nameNormalized = Normalizer.Normalize(userName);
			return db.Users.AnyAsync(x => x.NormalizedUserName == nameNormalized);
		}
	}
}