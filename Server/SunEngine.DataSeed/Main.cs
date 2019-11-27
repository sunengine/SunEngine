using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using SunEngine.Core.DataBase;
using SunEngine.Core.Models;

namespace SunEngine.DataSeed
{
    /// <summary>
    /// Class to seed database with initial data with 2 modes
    /// "init" (SeedInitialize) - seed roles, users, categories
    /// "seed" (SeedAddTestData) - seed test materials and comments 
    /// </summary>
    public class Main
    {
        private const string DataBaseConnectionFileName = "DataBaseConnection.json";
        public const string SeedCommand = "seed";
        private const string InitDir = "Init";

        private readonly string initDirPath;
        private readonly string providerName;
        private readonly string connectionString;
        private readonly string configDirectoryPath;


        public Main(string configDirectoryPath = "Config")
        {
            this.configDirectoryPath = Path.GetFullPath(configDirectoryPath);
            initDirPath = Path.GetFullPath(Path.Combine(configDirectoryPath, InitDir));
            string dbSettingsFile = Path.Combine(this.configDirectoryPath, DataBaseConnectionFileName);
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(dbSettingsFile, false, true)
                .Build();

            var dataBaseConfiguration = configuration.GetSection("DataBaseConnection");
            providerName = dataBaseConfiguration["Linq2dbProvider"];
            connectionString = dataBaseConfiguration["ConnectionString"];
        }


        /// <summary>
        /// Initialize database with roles, users, categories from config directory
        /// </summary>
        public void SeedInitialize()
        {
            CheckDbConnectionAndExitIfFailed();
            
            using var db = new DataBaseConnection(providerName, connectionString);

            var dataContainer = new InitialSeeder(initDirPath).Seed();
            var databaseSeeder = new DataBaseSeeder(db, dataContainer);


            databaseSeeder.SeedInitial();
        }

        /// <summary>
        /// Seed database with materials and comment for testing purposes
        /// </summary>
        public void SeedAddTestData(IList<string> catTokens, bool titleAppendCategoryName = false)
        {
            CheckDbConnectionAndExitIfFailed();
            
            const string seedCommandDots = SeedCommand + ":";
            if (catTokens.Contains(SeedCommand))
                catTokens[catTokens.IndexOf(SeedCommand)] = seedCommandDots + Category.RootName;
            catTokens = catTokens.Select(x => x.Substring(seedCommandDots.Length)).ToList();

            using DataBaseConnection db = new DataBaseConnection(providerName, connectionString);

            SeedTestData(db);


            void SeedTestData(DataBaseConnection db)
            {
                var dataContainer = new DataContainer
                {
                    Categories = db.Categories.ToList(),
                    Users = db.Users.ToList(),
                    currentMaterialId = db.Materials.Any() ? db.Materials.Max(x => x.Id) + 1 : 1,
                    currentCommentId = db.Comments.Any() ? db.Comments.Max(x => x.Id) + 1 : 1
                };

                var materialsSeeder = new MaterialsSeeder(dataContainer);

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
                        materialsSeeder.MinMaterialCount = materialsSeeder.MaxMaterialCount = materialsCount.Value;

                    if (commentsCount.HasValue)
                        materialsSeeder.CommentsCount = commentsCount.Value;

                    materialsSeeder.TitleAppendCategoryName = titleAppendCategoryName;

                    materialsSeeder.SeedCategoryAndSub(categoryName);
                }

                new DataBaseSeeder(db, dataContainer).SeedMaterials().PostSeedMaterials();
            }
        }

        protected void CheckDbConnectionAndExitIfFailed()
        {
            if (CheckDataBaseConnection(out Exception e))
                return;
            
            Console.WriteLine();
            Console.WriteLine(e);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Connection error.\nCheck if database exists and connection string correct.");
            Console.ResetColor();
            Console.WriteLine();
            Environment.Exit(1);
        }

        public bool PrintDbConnectionAvailability()
        {
            if (CheckDataBaseConnection(out Exception exception))
            {
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("Database is available.");
                Console.ResetColor();
                Console.WriteLine();
                return true;
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine(exception);
                Console.WriteLine();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Database is unavailable.");
                Console.ResetColor();
                Console.WriteLine();
                return false;
            }
        }


        protected bool CheckDataBaseConnection(out Exception exception)
        {
            exception = null;
            try
            {
                using var db = new DataBaseConnection(providerName, connectionString);
                using var cmd = db.CreateCommand();
                cmd.CommandText = "SELECT 100";
                int num = (int) cmd.ExecuteScalar();
            
                return true;
            }
            catch (Exception e)
            {
                exception = e;
                return false;
            }
        }
    }
}