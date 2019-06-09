using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Authorization;
using SunEngine.Core.Utils;

namespace SunEngine.DataSeed
{
    /// <summary>
    /// Seed users from json file from config dir to DataContainer
    /// </summary>
    public class UsersSeeder
    {
        private const string AllUsersDefaultPassword = "password";

        private readonly DataContainer dataContainer;
        private readonly string configDir;
        private readonly PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

        private JArray usersJArray;

        public UsersSeeder(DataContainer dataContainer, string configDir)
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

      


        private void SeedUser(JToken usersJ)
        {
            int maxNumber = 1;
            var repeat = usersJ["Repeat"];
            if (repeat != null)
            {
                maxNumber = (int) repeat;
            }

            int startNumber = 1;
            if (usersJ["StartNumber"] != null)
            {
                startNumber = int.Parse((string)usersJ["StartNumber"]);
                maxNumber += startNumber - 1;
            }
            
            for (int j = startNumber; j <= maxNumber; j++)
            {
                string name = ((string) usersJ["UserName"]).Replace("[n]", j.ToString());
                
                User user = new User
                {
                    Id = dataContainer.NextUserId(),
                    Email =  usersJ["Email"] != null 
                        ? ((string) usersJ["Email"]).Replace("[n]", j.ToString()) 
                        : name + "@email.email",
                    UserName = name,
                    EmailConfirmed = true,
                    PasswordHash = passwordHasher.HashPassword(null,
                        (string) (usersJ["Password"] ?? AllUsersDefaultPassword)),
                    SecurityStamp = string.Empty,
                    Information = ((string) usersJ["Information"])?.Replace("[n]", j.ToString()),
                    Link = ((string) usersJ["Link"])?.Replace("[n]", j.ToString()),
                    Photo = User.DefaultAvatar,
                    Avatar = User.DefaultAvatar
                };
                if(string.IsNullOrEmpty(user.Link))
                    user.Link = user.Id.ToString();
                
                MakeNormalizedUserFields(user);
                dataContainer.Users.Add(user);
            }
        }
        public void SeedUserRoles()
        {
            foreach (var userJ in usersJArray)
            {
                SeedUserRole(userJ);
            }
        }

        private void SeedUserRole(JToken userJ)
        {
            int maxNumber = 1;
            var repeat = userJ["Repeat"];
            if (repeat != null)
            {
                maxNumber = (int) repeat;
            }

            int startNumber = 1;
            if (userJ["StartNumber"] != null)
            {
                startNumber = int.Parse((string)userJ["StartNumber"]);
                maxNumber += startNumber - 1;
            }

            for (int j = startNumber; j <= maxNumber; j++)
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

        private void MakeNormalizedUserFields(User user)
        {
            user.NormalizedUserName = Normalizer.Normalize(user.UserName);
            user.NormalizedEmail = Normalizer.Normalize(user.Email);
        }
    }
}
