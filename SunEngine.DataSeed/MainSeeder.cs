using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SunEngine.Commons.DataBase;
using SunEngine.DataSeed.Seeder;

namespace SunEngine.DataSeed
{
    public class MainSeeder
    {
        private readonly string providerName;
        private readonly string connectionString;
        private readonly string configDirPath;

        public MainSeeder(string configDirPath = "Config")
        {
            this.configDirPath = configDirPath;
            string dbSettingsFile = Path.GetFullPath(Path.Combine(configDirPath, "DataBaseConnection.json"));
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(dbSettingsFile, optional: false, reloadOnChange: true)
                .Build();

            var dataBaseConfiguration = configuration.GetSection("DataBaseConnection");
            providerName = dataBaseConfiguration["Linq2dbProvider"];
            connectionString = dataBaseConfiguration["ConnectionString"];
        }

        public void SeedInitialize()
        {
            using (DataBaseConnection db = new DataBaseConnection(providerName, connectionString))
            {
                DataContainer dataContainer = new InitialSeeder(configDirPath).Seed();
                new DataBaseSeeder(db, dataContainer).SeedInitial();
            }
        }

        public void SeedAddTestData(IEnumerable<string> catTokens, bool? titleAppendCategoryName = null)
        {
            using (DataBaseConnection db = new DataBaseConnection(providerName, connectionString))
            {
                DataContainer dataContainer = new DataContainer
                {
                    Categories = db.Categories.ToList(),
                    Users = db.Users.ToList(),
                    currentMaterialId = db.Materials.Max(x=>x.Id)+1,
                    currentCommentId = db.Comments.Max(x=>x.Id)+1
                };
                
                MaterialsSeeder materialsSeeder = new MaterialsSeeder(dataContainer);

                foreach (var catToken in catTokens)
                {
                    var parts = catToken.Split(":");
                    var categoryName = parts[0];
                    int? materialsCount = null;
                    if (parts.Length > 1)
                        materialsCount = int.Parse(parts[1]);
                    int? commentsCount = null;
                    if (parts.Length > 2)
                        commentsCount = int.Parse(parts[2]);

                    if (materialsCount.HasValue)
                    {
                        materialsSeeder.MinMaterialCount = materialsCount.Value;
                        materialsSeeder.MaxMaterialCount = materialsCount.Value;
                    }

                    if (commentsCount.HasValue)
                        materialsSeeder.CommentsCount = commentsCount.Value;

                    if (titleAppendCategoryName.HasValue)
                        materialsSeeder.TitleAppendCategoryName = titleAppendCategoryName.Value;

                    materialsSeeder.SeedCategoryAndSub(categoryName);
                }
               
                new DataBaseSeeder(db, dataContainer).SeedMaterials().PostSeedMaterials();
            }
        }
        
        public void SeedAddTestData(string categoryName, int? materialsCount = null, int? commentsCount = null,
            bool? titleAppendCategoryName = null)
        {
            using (DataBaseConnection db = new DataBaseConnection(providerName, connectionString))
            {
                DataContainer dataContainer = new DataContainer
                {
                    Categories = db.Categories.ToList(),
                    Users = db.Users.ToList(),
                    currentMaterialId = db.Materials.Max(x=>x.Id)+1,
                    currentCommentId = db.Comments.Max(x=>x.Id)+1
                };
                
                MaterialsSeeder materialsSeeder = new MaterialsSeeder(dataContainer);
                if (materialsCount.HasValue)
                {
                    materialsSeeder.MinMaterialCount = materialsCount.Value;
                    materialsSeeder.MaxMaterialCount = materialsCount.Value;
                }

                if (commentsCount.HasValue)
                    materialsSeeder.CommentsCount = commentsCount.Value;

                if (titleAppendCategoryName.HasValue)
                    materialsSeeder.TitleAppendCategoryName = titleAppendCategoryName.Value;

                materialsSeeder.SeedCategoryAndSub(categoryName);
                new DataBaseSeeder(db, dataContainer).SeedMaterials().PostSeedMaterials();
            }
        }
    }
}