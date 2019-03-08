using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using SunEngine.Models;
using SunEngine.Models.Authorization;
using SunEngine.Security;
using SunEngine.Utils;

namespace DataSeed.Seeder
{
    public class UsersSeederJson
    {
        private const string AllUsersDefaultPassword = "password";

        private readonly DataContainer dataContainer;
        private readonly string configDir;
        private readonly PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

        private JArray usersJArray;

        public UsersSeederJson(DataContainer dataContainer, string configDir)
        {
            this.dataContainer = dataContainer;
            this.configDir = configDir;
        }

        public void SeedUsers()
        {
            string fileName = Path.GetFullPath(Path.Combine(configDir, "Users.json"));

            string jsonText = File.ReadAllText(fileName);
            usersJArray = JArray.Parse(jsonText);
            foreach (var userJ in usersJArray)
            {
                SeedUser(userJ);
            }
        }

        public void SeedUserRoles()
        {
            foreach (var userJ in usersJArray)
            {
                SeedUserRole(userJ);
            }
        }


        private void SeedUser(JToken usersJ)
        {
            int repeatCount = 1;
            var repeat = usersJ["Repeat"];
            if (repeat != null)
            {
                repeatCount = (int) repeat;
            }

            for (int j = 1; j <= repeatCount; j++)
            {
                User user = new User
                {
                    Id = dataContainer.NextUserId(),
                    Email = ((string) usersJ["Email"]).Replace("[n]", j.ToString()),
                    UserName = ((string) usersJ["UserName"]).Replace("[n]", j.ToString()),
                    EmailConfirmed = true,
                    PasswordHash = passwordHasher.HashPassword(null,
                        (string) (usersJ["Password"] ?? AllUsersDefaultPassword)),
                    SecurityStamp = string.Empty,
                    Information = ((string) usersJ["Information"]).Replace("[n]", j.ToString()),
                    Photo = User.DefaultAvatar,
                    Avatar = User.DefaultAvatar
                };
                user.SetDefaultLink();
                NormalizeUserFields(user);
                dataContainer.Users.Add(user);
            }
        }


        private void SeedUserRole(JToken userJ)
        {
            int repeatCount = 1;
            var repeat = userJ["Repeat"];
            if (repeat != null)
            {
                repeatCount = (int) repeat;
            }

            for (int j = 1; j <= repeatCount; j++)
            {
                string userName = ((string) userJ["UserName"]).Replace("[n]", j.ToString());

                JArray rolesJ = (JArray) userJ["Roles"];
                foreach (string roleName in rolesJ)
                {
                    UserRole userRole = new UserRole
                    {
                        UserId = dataContainer.Users.First(x => x.UserName == userName).Id,
                        RoleId = dataContainer.Roles.First(x => x.Name == roleName).Id
                    };

                    dataContainer.UserRoles.Add(userRole);
                }
            }
        }

        private void NormalizeUserFields(User user)
        {
            user.NormalizedUserName = Normalizer.Normalize(user.UserName);
            user.NormalizedEmail = Normalizer.Normalize(user.Email);
        }
    }
}