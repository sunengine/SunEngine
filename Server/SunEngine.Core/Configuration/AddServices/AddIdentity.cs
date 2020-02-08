using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.Configuration.Options;
using SunEngine.Core.DataBase;
using SunEngine.Core.Managers;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Authorization;

namespace SunEngine.Core.Configuration.AddServices
{
	public static class AddIdentityExtensions
	{
		public static void AddIdentity(this IServiceCollection services, IConfiguration configuration, IDataBaseFactory dataBaseFactory)
		{
			var passwordValidationOptions = configuration.GetSection("PasswordValidation").Get<PasswordValidationOptions>();
			var registerOptions = configuration.GetSection("Register").Get<RegisterOptions>();

			services.AddIdentity<User, Role>(
					options =>
					{
						options.Password.RequireDigit = passwordValidationOptions.RequireDigit;
						options.Password.RequireLowercase = passwordValidationOptions.RequireLowercase;
						options.Password.RequireNonAlphanumeric = passwordValidationOptions.RequireNonAlphanumeric;
						options.Password.RequireUppercase = passwordValidationOptions.RequireUppercase;
						options.Password.RequiredUniqueChars = passwordValidationOptions.RequiredUniqueChars;
						options.Password.RequiredLength = passwordValidationOptions.RequiredLength;
						options.User.RequireUniqueEmail = registerOptions.RequireUniqueEmail;
						options.User.AllowedUserNameCharacters = registerOptions.AllowedUserNameCharacters;
					})
				.AddLinqToDBStores<int>(dataBaseFactory)
				.AddUserManager<SunUserManager>()
				.AddRoleManager<SunRoleManager>()
				.AddDefaultTokenProviders();
		}
	}
}