using System;
using LinqToDB.Identity;
using LinqToDB.Mapping;
using SunEngine.Core.Models;
using SunEngine.Core.Models.Authorization;
using SunEngine.Core.Models.Materials;
using SunEngine.Core.Utils;


namespace SunEngine.Core.DataBase
{
    /// <summary>
    /// Mapping model to database schema for Linq2db DataBaseConnection class.
    /// </summary>
    public class DbMappingSchema : MappingSchema
    {
        public DbMappingSchema()
        {
            EntityDescriptorCreatedCallback = (schema, entityDescriptor) =>
            {
                entityDescriptor.TableName = entityDescriptor.TableName.ToSnakeCase();
                
                foreach (var entityDescriptorColumn in entityDescriptor.Columns)
                    entityDescriptorColumn.ColumnName = entityDescriptorColumn.ColumnName.ToSnakeCase();
            };
            
            SetConvertExpression<DateTime,DateTime>(dt => DateTime.SpecifyKind(dt, DateTimeKind.Utc));

            
            var mp = GetFluentMappingBuilder();
            mp.Entity<User>()
                .HasTableName("AspNetUsers")
                .HasIdentity(x => x.Id).HasPrimaryKey(x => x.Id)
                .Association(x => x.BanList, (x, y) => x.Id == y.UserId);

            mp.Entity<UserBanedUnit>()
                .HasTableName("UserBanedUnit")
                .HasIdentity(x => new {x.UserId, x.UserBanedId}).HasPrimaryKey(x => new {x.UserId, x.UserBanedId})
                .Association(x => x.User, x => x.UserId, x => x.Id)
                .Association(x => x.UserBaned, x => x.UserBanedId, x => x.Id);

            mp.Entity<Role>()
                .HasTableName("AspNetRoles")
                .HasIdentity(x => x.Id).HasPrimaryKey(x => x.Id)
                .Association(x => x.CategoryAccesses, x => x.Id, x => x.RoleId);

            mp.Entity<Category>()
                .HasTableName("Categories")
                .HasIdentity(x => x.Id).HasPrimaryKey(x => x.Id)
                .Association(x => x.Materials, x => x.Id, x => x.CategoryId)
                .Association(x => x.CacheSettings, x => x.CacheSettingsId, x => x.Id);

            mp.Entity<OperationKey>()
                .HasTableName("OperationKeys")
                .HasIdentity(x => x.OperationKeyId).HasPrimaryKey(x => x.OperationKeyId);

            mp.Entity<CategoryAccess>()
                .HasTableName("CategoryAccesses")
                .HasIdentity(x => x.Id).HasPrimaryKey(x => x.Id)
                .Association(x => x.Category, x => x.CategoryId, x => x.Id)
                .Association(x => x.CategoryOperationAccesses, x => x.Id, x => x.CategoryAccessId);

            mp.Entity<CategoryOperationAccess>()
                .HasTableName("CategoryOperationAccesses")
                .HasPrimaryKey(x => new {x.CategoryAccessId, x.OperationKeyId})
                .Association(x => x.OperationKey, x => x.OperationKeyId, x => x.OperationKeyId);

            mp.Entity<Material>()
                .HasTableName("Materials")
                .HasIdentity(x => x.Id).HasPrimaryKey(x => x.Id)
                .Association(x => x.Comments, (x, y) => x.Id == y.MaterialId)
                .Association(x => x.TagMaterials, (x, y) => x.Id == y.MaterialId)
                .Association(x => x.Author, x => x.AuthorId, x => x.Id)
                .Association(x => x.LastComment, x => x.LastCommentId, x => x.Id)
                .Association(x => x.Category, x => x.CategoryId, x => x.Id);

            mp.Entity<Comment>()
                .HasTableName("Comments")
                .HasIdentity(x => x.Id).HasPrimaryKey(x => x.Id)
                .Association(x => x.Author, x => x.AuthorId, x => x.Id)
                .Association(x => x.Material, x => x.MaterialId, x => x.Id);

            mp.Entity<Tag>()
                .HasTableName("Tags")
                .HasIdentity(x => x.Id).HasPrimaryKey(x => x.Id);

            mp.Entity<TagMaterial>()
                .HasTableName("TagMaterials")
                .HasPrimaryKey(x => new {x.TagId, x.MaterialId})
                .Association(x => x.Tag, x => x.TagId, x => x.Id)
                .Association(x => x.Material, x => x.MaterialId, x => x.Id);

            mp.Entity<IdentityUserRole<int>>()
                .HasTableName("AspNetUserRoles")
                .HasPrimaryKey(x => new {x.UserId, x.RoleId});

            mp.Entity<UserRole>()
                .HasTableName("AspNetUserRoles")
                .HasPrimaryKey(x => new {x.UserId, x.RoleId})
                .Association(x => x.User, x => x.UserId, x => x.Id)
                .Association(x => x.Role, x => x.RoleId, x => x.Id);

            mp.Entity<LongSession>()
                .HasTableName("LongSessions")
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.User, x => x.UserId, x => x.Id);

            mp.Entity<BlackListShortToken>()
                .HasTableName("BlackListShortTokens")
                .HasPrimaryKey(x => x.TokenId);

            mp.Entity<CacheSettings>()
                .HasTableName("CacheSettings")
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id);

            mp.Entity<CategoryCacheSettings>()
                .HasTableName("CategoryCacheSettings")
                .HasPrimaryKey(x => x.Id);

            mp.Entity<MenuItem>()
                .HasTableName("MenuItems")
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id);

            mp.Entity<CipherSecret>()
                .HasTableName("CipherSecrets")
                .HasPrimaryKey(x => x.Name);
            
            
        }
    }

    public static class DbColumnSizes
    {
        public const int SectionType_Name = 32;
        public const int SectionType_Title = 64;
        public const int Categories_Name = 64;
        public const int Categories_Title = 256;
        public const int Users_UserName = 64;
        public const int Users_Email = 64;
        public const int Users_Link = 32;
        public const int FileNameWithDirSize = 28;
        public const int Materials_Name = 32;
        public const int Materials_Title = 256;
        public const int Materials_Description = 256;
        public const int Tags_Name = 64;
        public const int Roles_Name = 64;
        public const int Roles_Title = 64;
        public const int OperationKey_Name = 100;
        public const int LongSessions_LongToken1 = 16;
        public const int LongSessions_LongToken2 = 16;
        public const int BlackListShortToken_TokenId = 16;
    }
}
