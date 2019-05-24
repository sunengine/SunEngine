using System;
using System.IO;
using System.Linq;
using NJsonSchema;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Authorization;
using SunEngine.Core.Security;
using SunEngine.Core.Utils;

namespace SunEngine.DataSeed
{
    /// <summary>
    /// Seed initial data Users, Roles, UserRoles, Categories, OperationKeys, SectionTypes
    /// from config dir to DataContainer.
    /// </summary>
    public class InitialSeeder
    {
        public const string CategoriesConfigDir = "Categories";
        public const string MenusConfigDir = "Menu";


        private readonly DataContainer dataContainer;

        private readonly UsersJsonSeeder usersJsonSeeder;

        private readonly string configDir;

        public InitialSeeder(string configDir)
        {
            this.configDir = configDir;
            dataContainer = new DataContainer();
            usersJsonSeeder = new UsersJsonSeeder(dataContainer, configDir);
        }

        public DataContainer Seed()
        {
            StartConsoleLog();

            SeedOperationKeys();

            SeedSectionTypes();

            SeedUsers();

            SeedCategories();

            SeedRoles();

            SeedUserRoles();
            
            SeedCacheSettings();

            SeedMenus();

            return dataContainer;
        }

        private void SeedMenus()
        {
            var path = Path.GetFullPath(Path.Combine(configDir, MenusConfigDir));
            MenuSeeder menuSeeder = new MenuSeeder(dataContainer,path);
            menuSeeder.Seed();
        }

        private void StartConsoleLog()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Data seed in memory:");
            Console.ResetColor();
        }

        private void SeedUsers()
        {
            Console.WriteLine("Users");

            usersJsonSeeder.SeedUsers();
        }

        private void SeedUserRoles()
        {
            Console.WriteLine("UsersRoles");

            usersJsonSeeder.SeedUserRoles();
        }

        private void SeedSectionTypes()
        {
            Console.WriteLine("SectionTypes");

            SectionType sectionTypeArticles = new SectionType
            {
                Id = dataContainer.NextSectionTypeId(),
                Name = "Articles",
                Title = "Articles"
            };
            dataContainer.SectionTypes.Add(sectionTypeArticles);

            SectionType sectionTypeForum = new SectionType
            {
                Id = dataContainer.NextSectionTypeId(),
                Name = "Forum",
                Title = "Forum"
            };
            dataContainer.SectionTypes.Add(sectionTypeForum);

            SectionType sectionTypeBlog = new SectionType
            {
                Id = dataContainer.NextSectionTypeId(),
                Name = "Blog",
                Title = "Blog"
            };
            dataContainer.SectionTypes.Add(sectionTypeBlog);
        }


        private void SeedRoles()
        {
            Console.WriteLine("Roles");

            string pathToUserGroupsConfig = Path.GetFullPath(Path.Combine(configDir,"Roles.json"));
            string pathToUserGroupsSchema = Path.GetFullPath("Resources/Roles.schema.json");
            JsonSchema4 schema = JsonSchema4.FromFileAsync(pathToUserGroupsSchema).GetAwaiter().GetResult();


            RolesFromJsonLoader fromJsonLoader =
                new RolesFromJsonLoader(dataContainer.Categories.ToDictionary(x => x.Name),
                    dataContainer.OperationKeys.ToDictionary(x => x.Name), schema);

            var json = File.ReadAllText(pathToUserGroupsConfig);

            fromJsonLoader.Seed(json);

            dataContainer.Roles = fromJsonLoader.roles;
            dataContainer.CategoryAccesses = fromJsonLoader.categoryAccesses;
            dataContainer.CategoryOperationAccesses = fromJsonLoader.categoryOperationAccesses;
        }

        private void SeedOperationKeys()
        {
            Console.WriteLine("OperationKeys");

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


        private void SeedCategories()
        {
            Console.WriteLine("Categories");

            SeedRootCategory();
            SeedCategoriesFromDirectory();
            DetectCategoriesParents();
        }

        private void DetectCategoriesParents()
        {
            foreach (var category in dataContainer.Categories)
            {
                if (category.ParentId.HasValue)
                    category.Parent = dataContainer.Categories.FirstOrDefault(x => x.Id == category.ParentId.Value);
            }
        }

        private void SeedRootCategory()
        {
            Category rootCategory = new Category
            {
                Id = 1,
                Name = Category.RootName,
                NameNormalized = Normalizer.Normalize(Category.RootName),
                Title = Category.RootName,
                SortNumber = 1
            };
            dataContainer.RootCategory = rootCategory;
            dataContainer.Categories.Add(rootCategory);
        }

        private void SeedCategoriesFromDirectory()
        {
            var fileNames = Directory.GetFiles(Path.GetFullPath(Path.Combine(configDir, CategoriesConfigDir)));

            CategoriesJsonSeeder categoriesJsonSeeder =
                new CategoriesJsonSeeder(dataContainer);
            foreach (var fileName in fileNames)
            {
                categoriesJsonSeeder.Seed(fileName);
            }
        }

        private void SeedCacheSettings()
        {
            dataContainer.CacheSettings = new CacheSettings
            {
                Id = 1,
                CachePolicy = CachePolicy.CustomPolicy,
                InvalidateCacheTime = 15
            };
        }
    }
}