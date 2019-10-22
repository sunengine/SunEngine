using System;
using System.IO;
using System.Linq;
using NJsonSchema;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Authorization;
using SunEngine.Core.Security;
using SunEngine.Core.Services;
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
        public const string ComponentsConfigDir = "Components";
        public const string MaterialsConfigDir = "Materials";


        private readonly DataContainer dataContainer = new DataContainer();

        private readonly UsersSeeder usersSeeder;

        private readonly string configDir;


        public InitialSeeder(string configDir)
        {
            this.configDir = configDir;
            usersSeeder = new UsersSeeder(dataContainer, configDir);
        }

        public DataContainer Seed()
        {
            StartConsoleLog();

            SeedOperationKeys();

            SeedUsers();

            SeedCategories();

            SeedMaterialsFromDir();

            SeedRoles();

            SeedUserRoles();

            SeedCacheSettings();

            SeedMenus();
            
            SeedComponents();

            SeedCipherSecrets();
            
            return dataContainer;
        }


        private void StartConsoleLog()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Data seed in memory:");
            Console.ResetColor();
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

        private void SeedUsers()
        {
            Console.WriteLine("Users");

            usersSeeder.SeedUsers();
        }

        private void SeedMaterialsFromDir()
        {
            var path = Path.GetFullPath(Path.Combine(configDir, MaterialsConfigDir));
            MaterialsSeederFromDir seedMaterialsFromDir = new MaterialsSeederFromDir(dataContainer, path);
            seedMaterialsFromDir.Seed();
        }

        private void SeedCategories()
        {
            Console.WriteLine("Categories");

            SeedRootCategory();
            SeedCategoriesFromDirectory();
            DetectCategoriesParents();

            void SeedRootCategory()
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


            void DetectCategoriesParents()
            {
                foreach (var category in dataContainer.Categories)
                {
                    if (category.ParentId.HasValue)
                        category.Parent = dataContainer.Categories.FirstOrDefault(x => x.Id == category.ParentId.Value);
                }
            }
        }

        private void SeedCategoriesFromDirectory()
        {
            var fileNames = Directory.GetFiles(Path.GetFullPath(Path.Combine(configDir, CategoriesConfigDir)));

            CategoriesSeeder categoriesSeeder =
                new CategoriesSeeder(dataContainer);
            foreach (var fileName in fileNames)
            {
                categoriesSeeder.Seed(fileName);
            }
        }

        private void SeedRoles()
        {
            Console.WriteLine("Roles");

            string pathToUserGroupsConfig = Path.GetFullPath(Path.Combine(configDir, "Roles.json"));
            string pathToUserGroupsSchema = Path.GetFullPath("Resources/Roles.schema.json");
            JsonSchema schema = JsonSchema.FromFileAsync(pathToUserGroupsSchema).GetAwaiter().GetResult();


            RolesFromJsonLoader fromJsonLoader =
                new RolesFromJsonLoader(dataContainer.Categories.ToDictionary(x => x.Name),
                    dataContainer.OperationKeys.ToDictionary(x => x.Name), schema);

            var json = File.ReadAllText(pathToUserGroupsConfig);

            fromJsonLoader.Seed(json);

            dataContainer.Roles = fromJsonLoader.roles;
            dataContainer.CategoryAccesses = fromJsonLoader.categoryAccesses;
            dataContainer.CategoryOperationAccesses = fromJsonLoader.categoryOperationAccesses;
        }

        private void SeedUserRoles()
        {
            Console.WriteLine("UsersRoles");

            usersSeeder.SeedUserRoles();
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

        private void SeedMenus()
        {
            var path = Path.GetFullPath(Path.Combine(configDir, MenusConfigDir));
            MenuSeeder menuSeeder = new MenuSeeder(dataContainer, path);
            menuSeeder.Seed();
        }

        private void SeedCipherSecrets()
        {
            var names = typeof(CipherSecrets).GetFields().Select(x => (string) x.GetValue(typeof(CipherSecrets)));

            foreach (var name in names)
            {
                dataContainer.CipherSecrets.Add(
                    new CipherSecret
                    {
                        Name = name,
                        Secret = CryptService.GenerateSecurityKeyString()
                    });
            }
        }

        private void SeedComponents()
        {
            var path = Path.GetFullPath(Path.Combine(configDir, ComponentsConfigDir));
            ComponentsSeeder componentsSeeder = new ComponentsSeeder(dataContainer, path);
            componentsSeeder.Seed();
        }
    }
}
