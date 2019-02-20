using System;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using NJsonSchema;
using SunEngine.Models;
using SunEngine.Models.Authorization;
using SunEngine.Security;
using SunEngine.Security.Authorization;
using SunEngine.Utils;

namespace DataSeedDev.Seeder
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
            
            Console.WriteLine("SectionTypes");

            SeedSectionTypes();
            
            Console.WriteLine("Categories");
            
            SeedCategories();
            
            Console.WriteLine("Roles");
            
            SeedUserGroups();
            
            Console.WriteLine("AssignUsersToRoles");
            
            RegisterUsers();
            
            AssignUsersToRoles();
            
            return dataContainer;
        }

        private void SeedSectionTypes()
        {
            SectionType sectionTypeArticles = new SectionType
            {
                Id = dataContainer.NextSectionTypeId(),
                Name = "Articles",
                Title = "Статьи"
            };
            dataContainer.SectionTypes.Add(sectionTypeArticles);
            
            SectionType sectionTypeForum = new SectionType
            {
                Id = dataContainer.NextSectionTypeId(),
                Name = "Forum",
                Title = "Форум"
            };
            dataContainer.SectionTypes.Add(sectionTypeForum);
            
            SectionType sectionTypeBlog = new SectionType
            {
                Id = dataContainer.NextSectionTypeId(),
                Name = "Blog",
                Title = "Блог"
            };
            dataContainer.SectionTypes.Add(sectionTypeBlog);
        }
        
        private void RegisterUsers()
        {
            Role registered = dataContainer.Roles.FirstOrDefault(x => x.Name == RoleNames.Registered);

            foreach (var user in dataContainer.Users)
            {
                UserRole ur = new UserRole
                {
                    UserId = user.Id,
                    RoleId = registered.Id
                };

                dataContainer.UserRoles.Add(ur);
            }
        }

        private void SeedUserGroups()
        {
            string pathToUserGroupsConfig = Path.GetFullPath("SeedConfig/UserGroups.json");
            string pathToUserGroupsSchema = Path.GetFullPath("SeedConfig/UserGroups.schema.json");
            JsonSchema4 schema = JsonSchema4.FromFileAsync(pathToUserGroupsSchema).GetAwaiter().GetResult();

            
            RolesFromJsonLoader fromJsonLoader = 
                new RolesFromJsonLoader(dataContainer.Categories.ToDictionary(x=>x.Name),
                    dataContainer.OperationKeys.ToDictionary(x=>x.Name), schema);

            var json = File.ReadAllText(pathToUserGroupsConfig);
            
            fromJsonLoader.Seed(json);

            dataContainer.Roles = fromJsonLoader.roles;
            dataContainer.CategoryAccesses = fromJsonLoader.categoryAccesses;
            dataContainer.CategoryOperationAccesses = fromJsonLoader.categoryOperationAccesses;
        }

        private void SeedOperationKeys()
        {
            var keys = OperationKeysContainer.GetAllOperationKeys();

            foreach (var key in keys)
            {
                var operationKey = new OperationKey
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
            var roleAdmin = dataContainer.Roles.FirstOrDefault(x => x.Name == RoleNames.Admin);

            UserRole ur = new UserRole
            {
                UserId = userAdmin.Id,
                RoleId = roleAdmin.Id
            };

            dataContainer.UserRoles.Add(ur);
            
            var userModerator = dataContainer.Users.FirstOrDefault(x => x.UserName == "Moderator");
            var roleModerator = dataContainer.Roles.FirstOrDefault(x => x.Name == "Moderator");

            ur = new UserRole
            {
                UserId = userModerator.Id,
                RoleId = roleModerator.Id
            };

            dataContainer.UserRoles.Add(ur);

            var testUser1 = dataContainer.Users.FirstOrDefault(x => x.UserName == "TestUser1");
            var testGroup1 = dataContainer.Roles.FirstOrDefault(x => x.Name == "TestGroup1");
            var testGroup2 = dataContainer.Roles.FirstOrDefault(x => x.Name == "TestGroup2");

            ur = new UserRole
            {
                UserId = testUser1.Id,
                RoleId = testGroup1.Id
            };

            dataContainer.UserRoles.Add(ur);

            ur = new UserRole
            {
                UserId = testUser1.Id,
                RoleId = testGroup2.Id
            };

            dataContainer.UserRoles.Add(ur);
        }

        private void NormalizeUserFields(User user)
        {
            user.NormalizedUserName = FieldNormalizer.Normalize(user.UserName);
            user.NormalizedEmail = FieldNormalizer.Normalize(user.Email);
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
                Name = Category.RootName,
                Title = "Корень",
                IsCategoriesContainer = true,
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