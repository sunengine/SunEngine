using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Models.UserGroups;
using SunEngine.Commons.Services;

namespace SunEngine.Configuration
{
    public static class AddIdentityExtensions
    {
        public static void AddIdentity(this IServiceCollection services, DataBaseFactory dataBaseFactory)
        {
            services.AddIdentity<User, UserGroupDB>(
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
                        const string other = "-";

                        options.User.AllowedUserNameCharacters =
                            engChars + engChars.ToUpper() + rusChars + rusChars.ToUpper() + numbers + other;
                    })
                .AddLinqToDBStores<int>(dataBaseFactory)
                .AddUserManager<MyUserManager>()
                .AddRoleManager<MyRoleManager>()
                .AddDefaultTokenProviders();
        }
    }
}