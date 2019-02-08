using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.DataProvider.MySql;
using LinqToDB.Identity;
using SunEngine.Commons.Models;
using SunEngine.Commons.Models.UserGroups;

namespace SunEngine.Commons.DataBase
{
    public class DataBaseConnection : IdentityDataConnection<User, UserGroupDB, int>
    {
        public DataBaseConnection(string providerName, string connectionString, MyMappingSchema mappingSchema) : base(
            providerName, connectionString)
        {
            AddMappingSchema(mappingSchema);
        }
        
        public DataBaseConnection(IDataProvider dataProvider, string connectionString, MyMappingSchema mappingSchema) : base(
            dataProvider, connectionString)
        {
            AddMappingSchema(mappingSchema);
        }
        
        public DataBaseConnection(IDataProvider dataProvider, string connectionString) : base(
            dataProvider, connectionString)
        {
            AddMappingSchema(new MyMappingSchema());
        }
        
        public DataBaseConnection(string providerName, string connectionString) : base(
            providerName, connectionString)
        {
            AddMappingSchema(new MyMappingSchema());
        }

        public ITable<Material> Materials => GetTable<Material>();
        public IQueryable<Material> MaterialsNotDeleted => GetTable<Material>().Where(x => !x.IsDeleted);

        public ITable<Message> Messages => GetTable<Message>();
        public IQueryable<Message> MessagesNotDeleted => GetTable<Message>().Where(x => !x.IsDeleted);

        public ITable<Tag> Tags => GetTable<Tag>();
        public ITable<TagSynonymGroup> TagSynonyms => GetTable<TagSynonymGroup>();
        public ITable<TagMaterial> TagMaterials => GetTable<TagMaterial>();
        
        public ITable<Category> Categories => GetTable<Category>();
        public IQueryable<Category> CategoriesVisible => GetTable<Category>().Where(x => !x.IsHidden && !x.IsDeleted);

        
        public ITable<OperationKeyDB> OperationKeys => GetTable<OperationKeyDB>();
        public ITable<CategoryAccessDB> CategoryAccess => GetTable<CategoryAccessDB>();
        public ITable<CategoryOperationAccessDB> CategoryOperationAccess => GetTable<CategoryOperationAccessDB>();
        public ITable<User> Users => GetTable<User>();
        public ITable<UserGroupDB> UserGroups => GetTable<UserGroupDB>();
        
        public ITable<UserToGroup> UserToGroups => GetTable<UserToGroup>();
        
        public ITable<UserBanedUnit> UserBanedUnits => GetTable<UserBanedUnit>();
        
        public ITable<LongSession> LongSessions => GetTable<LongSession>();


        public void UpdateSequence(string tableName,string keyName)
        {
            if(DataProvider.Name.StartsWith("Postgre"))
                this.Execute($"SELECT setval(pg_get_serial_sequence('\"{tableName}\"', '{keyName}'), coalesce(max(tbl.\"{keyName}\"),0) + 1, false) FROM \"{tableName}\" as tbl;");
        }
    }
}