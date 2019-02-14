using System;
using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using SunEngine.DataBase;

namespace DataSeedDev.Seeder
{
    public class DataBaseSeeder
    {
        private readonly DataContainer dataContainer;
        private readonly DataBaseConnection db;

        public DataBaseSeeder(DataBaseConnection db, DataContainer dataContainer)
        {
            this.dataContainer = dataContainer;
            this.db = db;
        }

        public DataBaseSeeder Seed()
        {
            BulkCopyOptions options = new BulkCopyOptions
            {
                CheckConstraints = false,
                BulkCopyType = BulkCopyType.Default,
                KeepIdentity = true
            };
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Data seed in DataBase:");
            Console.ResetColor();

            
            Console.WriteLine("Categories");
            db.BulkCopy(options,dataContainer.Categories);

            Console.WriteLine("Users");
            db.BulkCopy(options,dataContainer.Users);
            
            Console.WriteLine("Materials");
            db.BulkCopy(options,dataContainer.Materials);
            
            
            Console.WriteLine("Messages");
            db.BulkCopy(options,dataContainer.Messages);
            
            Console.WriteLine("Roles");
            db.BulkCopy(options,dataContainer.Roles);
            
            Console.WriteLine("UserRoles");
            db.BulkCopy(options,dataContainer.UserRoles);
            
            Console.WriteLine("OperationKeys");
            db.BulkCopy(options,dataContainer.OperationKeys);
            
            Console.WriteLine("CategoryAccesses");
            db.BulkCopy(options,dataContainer.CategoryAccesses);
            
            Console.WriteLine("CategoryOperationAccesses");
            db.BulkCopy(options,dataContainer.CategoryOperationAccesses);

            RunDbCpecificCode();
            
            /*db.BulkCopy(options,dataContainer.Categories);
            db.BulkCopy(dataContainer.Users);
            db.BulkCopy(dataContainer.Materials);
            db.BulkCopy(dataContainer.Messages);
            db.BulkCopy(dataContainer.UserGroups);
            db.BulkCopy(dataContainer.IdentityUserRoles);
            db.BulkCopy(dataContainer.OperationKeys);
            db.BulkCopy(dataContainer.CategoryAccesses);
            db.BulkCopy(dataContainer.CategoryOperationAccesses);*/
            
            /*BulkCopyOptions options = new BulkCopyOptions
            {
                CheckConstraints = false,
                 
            };
            db.BulkCopy(options,dataContainer.Categories);
            db.BulkCopy(options,dataContainer.Users);
            db.BulkCopy(options,dataContainer.Materials);
            db.BulkCopy(options,dataContainer.Messages);
            db.BulkCopy(options,dataContainer.UserGroups);
            db.BulkCopy(options,dataContainer.IdentityUserRoles);
            db.BulkCopy(options,dataContainer.OperationKeys);
            db.BulkCopy(options,dataContainer.CategoryAccesses);
            db.BulkCopy(options,dataContainer.CategoryOperationAccesses);*/

            return this;
        }

        public void RunDbCpecificCode()
        {
            if (db.DataProvider.Name.StartsWith("PostgreSQL"))
            {
                Console.WriteLine("Update sequences for PostgreSQL");

                string[] tablesWithSequenceIds =
                    {"Categories", "AspNetUsers", "Materials", "Messages", "AspNetRoles", "CategoryAccesses", "Tags"};

                foreach (string tableName in tablesWithSequenceIds)
                {
                    //Console.WriteLine($"Renew sequence of table: '{tableName}'");
                    db.Execute($"SELECT setval(pg_get_serial_sequence('\"{tableName}\"', 'Id'), coalesce(max(tbl.\"Id\"),0) + 1, false) FROM \"{tableName}\" as tbl;");
                }
                
                db.Execute("SELECT setval(pg_get_serial_sequence('\"OperationKeys\"', 'OperationKeyId'), coalesce(max(tbl.\"OperationKeyId\"),0) + 1, false) FROM \"OperationKeys\" as tbl;");
            }
        }

        public DataBaseSeeder PostSeed()
        {
            db.Materials.Where(x => x.Messages.Count > 0)
                .Set(x=>x.LastMessageId,
                    x=>x.Messages.OrderByDescending(y=>y.PublishDate).First().Id)
                .Update();
            
            return this;
        }
    }
}