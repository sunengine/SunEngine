using System;
using System.IO;
using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.AspNetCore.Identity;
using NJsonSchema;
using SunEngine.Commons;
using SunEngine.Commons.DataBase;
using SunEngine.Commons.Models;
using SunEngine.Commons.Models.UserGroups;
using SunEngine.Commons.Services;
using SunEngine.Commons.Utils;

namespace SunEngine.Seeder
{
    public class LocalSeeder
    {
        private const string AllUsersPassword = "password";

        private readonly DataContainer dataContainer;

        private readonly MaterialsSeeder materialsSeeder;



        public LocalSeeder()
        {

            dataContainer = new DataContainer();
            materialsSeeder = new MaterialsSeeder(dataContainer);
        }

        public DataContainer Seed()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Data seed in memory:");
            Console.ResetColor();
            
            Console.WriteLine("OperationKeys");
            
            SeedOperationKeys();
            
            Console.WriteLine("Users");
            
            SeedUsers();
            
            Console.WriteLine("Categories");
            
            SeedCategories();
            
            Console.WriteLine("UserGroups");
            
            SeedUserGroups();
            
            Console.WriteLine("AssignUsersToRoles");
            
            RegisterUsers();
            
            AssignUsersToRoles();
            
            return dataContainer;
        }

        private void RegisterUsers()
        {
            UserGroupDB registered = dataContainer.UserGroups.FirstOrDefault(x => x.Name == "Registered");

            foreach (var user in dataContainer.Users)
            {
                UserToGroup ur = new UserToGroup
                {
                    UserId = user.Id,
                    RoleId = registered.Id
                };

                dataContainer.UserToGroups.Add(ur);
            }
        }

        private void SeedUserGroups()
        {
            string pathToUserGroupsConfig = Path.GetFullPath("SeedConfig/UserGroups.json");
            string pathToUserGroupsSchema = Path.GetFullPath("SeedConfig/UserGroups.schema.json");
            JsonSchema4 schema = JsonSchema4.FromFileAsync(pathToUserGroupsSchema).GetAwaiter().GetResult();

            
            UserGroupsLoaderFromJson loader = 
                new UserGroupsLoaderFromJson(dataContainer.Categories.ToDictionary(x=>x.Name),
                    dataContainer.OperationKeys.ToDictionary(x=>x.Name), schema);

            var json = File.ReadAllText(pathToUserGroupsConfig);
            
            loader.Seed(json);

            dataContainer.UserGroups = loader.userGroups;
            dataContainer.CategoryAccesses = loader.categoryAccesses;
            dataContainer.CategoryOperationAccesses = loader.categoryOperationAccesses;
        }

        private void SeedOperationKeys()
        {
            var keys = OperationKeysContainer.GetAllOperationKeys();

            foreach (var key in keys)
            {
                var operationKey = new OperationKeyDB
                {
                    OperationKeyId = dataContainer.NextOperationKeyId(),
                    Name = key
                };

                dataContainer.OperationKeys.Add(operationKey);
            }

        }

        private void AssignUsersToRoles()
        {
            var userAdmin = dataContainer.Users.FirstOrDefault(x => x.UserName == "Admin");
            var roleAdmin = dataContainer.UserGroups.FirstOrDefault(x => x.Name == "Admin");

            UserToGroup ur = new UserToGroup
            {
                UserId = userAdmin.Id,
                RoleId = roleAdmin.Id
            };

            dataContainer.UserToGroups.Add(ur);
            
            var userModerator = dataContainer.Users.FirstOrDefault(x => x.UserName == "Moderator");
            var roleModerator = dataContainer.UserGroups.FirstOrDefault(x => x.Name == "Moderator");

            ur = new UserToGroup
            {
                UserId = userModerator.Id,
                RoleId = roleModerator.Id
            };

            dataContainer.UserToGroups.Add(ur);

            var testUser1 = dataContainer.Users.FirstOrDefault(x => x.UserName == "TestUser1");
            var testGroup1 = dataContainer.UserGroups.FirstOrDefault(x => x.Name == "TestGroup1");
            var testGroup2 = dataContainer.UserGroups.FirstOrDefault(x => x.Name == "TestGroup2");

            ur = new UserToGroup
            {
                UserId = testUser1.Id,
                RoleId = testGroup1.Id
            };

            dataContainer.UserToGroups.Add(ur);

            ur = new UserToGroup
            {
                UserId = testUser1.Id,
                RoleId = testGroup2.Id
            };

            dataContainer.UserToGroups.Add(ur);
        }

        private void NormalizeUserFields(User user)
        {
            user.NormalizedUserName = Normalizer.Singleton.Normalize(user.UserName);
            user.NormalizedEmail = Normalizer.Singleton.Normalize(user.Email);
        }

        private void SeedUsers()
        {
            var hasher = new PasswordHasher<User>();
            var passwordHash = hasher.HashPassword(null, AllUsersPassword);

            User admin = new User
            {
                Id = dataContainer.NextUserId(),
                Email = $"admin@email.ru",
                UserName = "Admin",
                EmailConfirmed = true,
                PasswordHash = passwordHash,
                SecurityStamp = string.Empty,
                Information = "Администратор, информация о пользователе",
                Photo = User.DefaultAvatar,
                Avatar = User.DefaultAvatar
            };
            admin.SetDefaultLink();
            NormalizeUserFields(admin);


            dataContainer.Users.Add(admin);

            User moderator = new User
            {
                Id = dataContainer.NextUserId(),
                Email = $"moderator@email.ru",
                UserName = "Moderator",
                EmailConfirmed = true,
                PasswordHash = passwordHash,
                SecurityStamp = string.Empty,
                Information = "Модератор, информация о пользователе",
                Photo = User.DefaultAvatar,
                Avatar = User.DefaultAvatar
            };
            NormalizeUserFields(moderator);
            moderator.SetDefaultLink();

            dataContainer.Users.Add(moderator);

            User testUser1 = new User
            {
                Id = dataContainer.NextUserId(),
                Email = $"testUser1@email.ru",
                UserName = "TestUser1",
                EmailConfirmed = true,
                PasswordHash = passwordHash,
                SecurityStamp = string.Empty,
                Information = "Тестовый пользователь, информация о пользователе",
                Photo = User.DefaultAvatar,
                Avatar = User.DefaultAvatar
            };
            testUser1.SetDefaultLink();
            NormalizeUserFields(testUser1);

            dataContainer.Users.Add(testUser1);


            for (int i = 1; i <= 5; i++)
            {
                User user = new User
                {
                    Id = dataContainer.NextUserId(),
                    Email = $"user{i}@email.ru",
                    UserName = $"User" + i,
                    EmailConfirmed = true,
                    PasswordHash = passwordHash,
                    SecurityStamp = string.Empty,
                    Information = "Информация о пользователе",
                    Photo = User.DefaultAvatar,
                    Avatar = User.DefaultAvatar
                };
                user.SetDefaultLink();
                NormalizeUserFields(user);

                if (i <= 2)
                {
                    user.SetDefaultLink();
                }
                else
                {
                    user.Link = "User" + i;
                }

                dataContainer.Users.Add(user);
            }
        }

        private void SeedCategories()
        {
            SeedRootCategory();
            SeedCategoriesFromDirectory();
        }

        private void SeedRootCategory()
        {
            int id = dataContainer.NextCategoryId();
            Category rootCategory = new Category
            {
                Id = id,
                Name = "Root",
                Title = "Корень",
                IsFolder = true,
                SortNumber = id
            };
            dataContainer.RootCategory = rootCategory;
            dataContainer.Categories.Add(rootCategory);
        }



        private void SeedCategoriesFromDirectory()
        {
            //string pathToCategoriesStartConfigFolder = Path.GetFullPath("CategoriesStartConfig");//Path.Combine(hostingEnvironment.ContentRootPath, "CategoriesStartConfig");

            var fileNames = Directory.GetFiles(Path.GetFullPath("SeedConfig/CategoriesStartConfig"));

            SeederCategoriesFromJson seederCategoriesFromJson =
                new SeederCategoriesFromJson(dataContainer, materialsSeeder);
            foreach (var fileName in fileNames)
            {
                seederCategoriesFromJson.Seed(fileName);
            }
        }
    }
}