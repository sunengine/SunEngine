using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Core.DataBase;
using SunEngine.Core.Managers;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Authorization;

namespace SunEngine.Core.Configuration.AddServices
{
	public static class AddIdentityExtensions
	{
		public static void AddIdentity(this IServiceCollection services, IDataBaseFactory dataBaseFactory)
		{
			services.AddIdentity<User, Role>(
					options =>
					{
						options.Password.RequireDigit = false;
						options.Password.RequireLowercase = false;
						options.Password.RequireNonAlphanumeric = false;
						options.Password.RequireUppercase = false;
						options.Password.RequiredUniqueChars = 2;
						options.Password.RequiredLength = 6;
						options.User.RequireUniqueEmail = true;

						const string engChars = "abcdefghijklmnopqrstuvwxyz";
						const string rusChars = "абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
						const string numbers = "0123456789";
						const string other = " -";

						options.User.AllowedUserNameCharacters =
							engChars + engChars.ToUpper() + rusChars + rusChars.ToUpper() + numbers + other;
					})
				.AddLinqToDBStores<int>(dataBaseFactory)
				.AddUserManager<SunUserManager>()
				.AddRoleManager<SunRoleManager>()
				.AddDefaultTokenProviders();
		}
	}
}