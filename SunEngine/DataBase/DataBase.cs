using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.Identity;
using SunEngine.Models;
using SunEngine.Models.Authorization;
using SunEngine.Models.Materials;

namespace SunEngine.DataBase
{
    public class DataBaseConnection : IdentityDataConnection<User, UserGroup, int>
    {
        public DataBaseConnection(string providerName, string connectionString, SunEngineMappingSchema mappingSchema) : base(
            providerName, connectionString)
        {
            AddMappingSchema(mappingSchema);
        }
        
        public DataBaseConnection(IDataProvider dataProvider, string connectionString, SunEngineMappingSchema mappingSchema) : base(
            dataProvider, connectionString)
        {
            AddMappingSchema(mappingSchema);
        }
        
        public DataBaseConnection(IDataProvider dataProvider, string connectionString) : base(
            dataProvider, connectionString)
        {
            AddMappingSchema(new SunEngineMappingSchema());
        }
        
        public DataBaseConnection(string providerName, string connectionString) : base(
            providerName, connectionString)
        {
            AddMappingSchema(new SunEngineMappingSchema());
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

        
        public ITable<OperationKey> OperationKeys => GetTable<OperationKey>();
        public ITable<CategoryAccess> CategoryAccess => GetTable<CategoryAccess>();
        public ITable<CategoryOperationAccess> CategoryOperationAccess => GetTable<CategoryOperationAccess>();
        public ITable<User> Users => GetTable<User>();
        public ITable<UserGroup> UserGroups => GetTable<UserGroup>();
        
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