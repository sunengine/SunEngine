using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using Microsoft.Extensions.Configuration;
using SunEngine.Core.DataBase;
using SunEngine.Core.Errors;
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


        private readonly string providerName;
        private readonly string connectionString;
        private readonly string configDirectoryPath;
        

        public Main(string configDirectoryPath = "Config")
        {
            this.configDirectoryPath = configDirectoryPath;
            string dbSettingsFile = Path.GetFullPath(Path.Combine(configDirectoryPath, DataBaseConnectionFileName));
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
            using (var db = new DataBaseConnection(providerName, connectionString))
            {
                var dataContainer = new InitialSeeder(configDirectoryPath).Seed();
                var databaseSeeder = new DataBaseSeeder(db, dataContainer);

                try
                {
                    databaseSeeder.SeedInitial();
                }
                catch (DbException e)
                {
                    throw new SunDataBaseException(
                        "Exception happened in data seed process. " +
                        "Check that last migrations were done('migrate' argument).", e);
                }
                catch (SocketException e)
                {
                    throw new SunDataBaseException("The connection could not be made. " +
                                                             "Check the database you are trying to connect exists.", e);
                }
            }
        }

        /// <summary>
        /// Seed database with materials and comment for testing purposes
        /// </summary>
        public void SeedAddTestData(IList<string> catTokens, bool titleAppendCategoryName = false)
        {
            const string seedCommandDots = SeedCommand + ":";
            if (catTokens.Contains(SeedCommand))
                catTokens[catTokens.IndexOf(SeedCommand)] = seedCommandDots + Category.RootName;
            catTokens = catTokens.Select(x => x.Substring(seedCommandDots.Length)).ToList();

            using (DataBaseConnection db = new DataBaseConnection(providerName, connectionString))
            {
                try
                {
                    SeedTestData(db);
                }
                catch (DbException e)
                {
                    throw new SunDataBaseException(
                        "Exception happened in data seed process. " +
                        "Check that last migrations were done('migrate' argument) and system initialized ('init' argument).",
                        e);
                }
                catch (SocketException e)
                {
                    throw new SunDataBaseException("The connection could not be made. " +
                                                             "Check the database you are trying to connect exists.", e);
                }
            }


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


        /// <summary>
        /// Check is Database exists and connection working
        /// </summary>
        public bool CheckConnection()
        {
            using (var db = new DataBaseConnection(providerName, connectionString))
            {
                try
                {
                    db.Connection.Open();
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine("Database is available.");
                    Console.ResetColor();
                    return true;
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Database is unavailable.");
                    Console.ResetColor();
                    Console.WriteLine(exception);
                    return false;
                }
                finally
                {
                    db.Connection.Close();
                }
            }
        }
    }
}
