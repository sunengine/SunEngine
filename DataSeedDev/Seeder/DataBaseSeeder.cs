using System.Data.Odbc;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Xml.XPath;
using LinqToDB;
using LinqToDB.Data;
using SunEngine.Commons.DataBase;

namespace SunEngine.Seeder
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
            
            db.BulkCopy(options,dataContainer.Categories);
            db.BulkCopy(options,dataContainer.Users);
            db.BulkCopy(options,dataContainer.Materials);
            db.BulkCopy(options,dataContainer.Messages);
            db.BulkCopy(options,dataContainer.UserGroups);
            db.BulkCopy(options,dataContainer.IdentityUserRoles);
            db.BulkCopy(options,dataContainer.OperationKeys);
            db.BulkCopy(options,dataContainer.CategoryAccesses);
            db.BulkCopy(options,dataContainer.CategoryOperationAccesses);
            
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