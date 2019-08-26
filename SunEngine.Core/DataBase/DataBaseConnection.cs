using System.Linq;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using LinqToDB.Identity;
using LinqToDB.Mapping;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Authorization;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Utils;


namespace SunEngine.Core.DataBase
{
    /// <summary>
    /// Main Linq2db class to make database operations
    /// </summary>
    public class DataBaseConnection : IdentityDataConnection<User, Role, int>
    {
        public DataBaseConnection(string providerName, string connectionString, DbMappingSchema mappingSchema) : base(
            providerName, connectionString)
        {
            AddMappingSchema(mappingSchema);
        }

        public DataBaseConnection(
            IDataProvider dataProvider, string connectionString,
            MappingSchema mappingSchema) : base(
            dataProvider, connectionString)
        {
            AddMappingSchema(mappingSchema);
        }

        public DataBaseConnection(IDataProvider dataProvider, string connectionString) : base(
            dataProvider, connectionString)
        {
            AddMappingSchema(new DbMappingSchema());
        }

        public DataBaseConnection(string providerName, string connectionString) : base(
            providerName, connectionString)
        {
            AddMappingSchema(new DbMappingSchema());
        }

        public ITable<Material> Materials => GetTable<Material>();

        public IQueryable<Material> MaterialsVisible =>
            GetTable<Material>()
                .Where(x => x.DeletedDate == null && !x.IsHidden && !x.Category.IsHidden && x.Category.DeletedDate == null);

        public ITable<Comment> Comments => GetTable<Comment>();

        public ITable<Tag> Tags => GetTable<Tag>();
        public ITable<TagSynonymGroup> TagSynonyms => GetTable<TagSynonymGroup>();
        public ITable<TagMaterial> TagMaterials => GetTable<TagMaterial>();

        public ITable<Category> Categories => GetTable<Category>();

        public ITable<OperationKey> OperationKeys => GetTable<OperationKey>();
        public ITable<CategoryAccess> CategoryAccess => GetTable<CategoryAccess>();
        public ITable<CategoryOperationAccess> CategoryOperationAccess => GetTable<CategoryOperationAccess>();
        
        public new ITable<User> Users => GetTable<User>();
        public new ITable<Role> Roles => GetTable<Role>();

        public new ITable<UserRole> UserRoles => GetTable<UserRole>();

        public ITable<UserBanedUnit> UserBanedUnits => GetTable<UserBanedUnit>();

        public ITable<LongSession> LongSessions => GetTable<LongSession>();

        public ITable<BlackListShortToken> BlackListShortTokens => GetTable<BlackListShortToken>();

        public ITable<CacheSettings> CacheSettings => GetTable<CacheSettings>();

        public ITable<MenuItem> MenuItems => GetTable<MenuItem>();

        public ITable<CipherSecret> CipherSecrets => GetTable<CipherSecret>();

        
        public void UpdateSequence(string tableName, string keyName)
        {
            if (IsPostgres())
                this.Execute(
                    $"SELECT setval(pg_get_serial_sequence('\"{tableName}\"', '{keyName}'), coalesce(max(tbl.\"{keyName}\"),0) + 1, false) FROM \"{tableName}\" as tbl;");
        }

        public bool IsPostgres()
        {
            return DataProvider.Name.StartsWith("Postgre");
        }
    }
}
