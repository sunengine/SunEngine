using System;
using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using SunEngine.Core.DataBase;
using SunEngine.Core.Utils;


namespace SunEngine.DataSeed
{
    /// <summary>
    /// Seed data from DataContainer to database
    /// </summary>
    public class DataBaseSeeder
    {
        private readonly DataContainer dataContainer;
        private readonly DataBaseConnection db;

        public DataBaseSeeder(DataBaseConnection db, DataContainer dataContainer)
        {
            this.dataContainer = dataContainer;
            this.db = db;
        }

        public DataBaseSeeder SeedInitial()
        {
            BulkCopyOptions options = new BulkCopyOptions
            {
                CheckConstraints = false,
                BulkCopyType = BulkCopyType.Default,
                KeepIdentity = true
            };

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Data seed Initial in DataBase:");
            Console.ResetColor();

            Console.WriteLine("Categories");
            db.BulkCopy(options, dataContainer.Categories);

            Console.WriteLine("Users");
            db.BulkCopy(options, dataContainer.Users);
           
            Console.WriteLine("Roles");
            db.BulkCopy(options, dataContainer.Roles);

            Console.WriteLine("UserRoles");
            db.BulkCopy(options, dataContainer.UserRoles);

            Console.WriteLine("OperationKeys");
            db.BulkCopy(options, dataContainer.OperationKeys);

            Console.WriteLine("CategoryAccesses");
            db.BulkCopy(options, dataContainer.CategoryAccesses);

            Console.WriteLine("CategoryOperationAccesses");
            db.BulkCopy(options, dataContainer.CategoryOperationAccesses);
            
            Console.WriteLine("MenuItems");
            db.BulkCopy(options, dataContainer.MenuItems);
            
            Console.WriteLine("CipherSecrets");
            db.BulkCopy(options, dataContainer.CipherSecrets);

            if (dataContainer.CacheSettings != null)
            {
                Console.WriteLine("Cache Settings");
                db.Insert(dataContainer.CacheSettings);
            }

            RunDbCpecificCode();

            return this;
        }
        
        public DataBaseSeeder SeedMaterials()
        {
            BulkCopyOptions options = new BulkCopyOptions
            {
                CheckConstraints = false,
                BulkCopyType = BulkCopyType.Default,
                KeepIdentity = true
            };

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Data seed Materials in DataBase:");
            Console.ResetColor();

            Console.WriteLine("Materials");
            db.BulkCopy(options, dataContainer.Materials);

            Console.WriteLine("Comments");
            db.BulkCopy(options, dataContainer.Comments);

            RunDbCpecificCode();

            return this;
        }

        public void RunDbCpecificCode()
        {
            if (db.DataProvider.Name.StartsWith("PostgreSQL"))
            {
                Console.WriteLine("Update sequences for PostgreSQL");

                string[] tablesWithSequenceIds =
                {
                    "Categories", "AspNetUsers", "Materials", "Comments", "AspNetRoles",
                    "CategoryAccesses", "Tags", "MenuItems"
                };

                foreach (string tableName in tablesWithSequenceIds)
                {
                    //Console.WriteLine($"Renew sequence of table: '{tableName}'");
                    db.Execute(
                        $"SELECT setval(pg_get_serial_sequence('\"{tableName.ToSnakeCase()}\"', 'Id'), coalesce(max(tbl.\"{"Id".ToSnakeCase()}\"),0) + 1, false) FROM \"{tableName.ToSnakeCase()}\" as tbl;");
                }

                db.Execute(
                    $"SELECT setval(pg_get_serial_sequence('\"{"OperationKeys".ToSnakeCase()}\"', '{"OperationKeyId".ToSnakeCase()}'), coalesce(max(tbl.\"{"OperationKeyId".ToSnakeCase()}\"),0) + 1, false) FROM \"{"OperationKeys".ToSnakeCase()}\" as tbl;");
            }
        }

        public DataBaseSeeder PostSeedMaterials()
        {
            db.Materials.Where(x => x.Comments.Count > 0)
                .Set(x => x.LastCommentId,
                    x => x.Comments.OrderByDescending(y => y.PublishDate).First().Id)
                .Update();

            return this;
        }
    }
}
