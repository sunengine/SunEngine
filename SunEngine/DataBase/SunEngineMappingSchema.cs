using LinqToDB.Identity;
using LinqToDB.Mapping;
using SunEngine.Models;
using SunEngine.Models.Authorization;
using SunEngine.Models.Materials;

namespace SunEngine.DataBase
{
    public class SunEngineMappingSchema : MappingSchema
    {
        public SunEngineMappingSchema()
        {
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

            mp.Entity<UserGroup>()
                .HasTableName("AspNetRoles")
                .HasIdentity(x => x.Id).HasPrimaryKey(x => x.Id)
                .Association(x=>x.CategoryAccesses,x=>x.Id,x=>x.UserGroupId);

            mp.Entity<Category>()
                .HasTableName("Categories")
                .HasIdentity(x => x.Id).HasPrimaryKey(x => x.Id)
                .Association(x => x.Materials, x => x.Id, x => x.CategoryId);

            mp.Entity<OperationKey>()
                .HasTableName("OperationKeys")
                .HasIdentity(x => x.OperationKeyId).HasPrimaryKey(x => x.OperationKeyId);

            mp.Entity<CategoryAccess>()
                .HasTableName("CategoryAccesses")
                .HasIdentity(x => x.Id).HasPrimaryKey(x => x.Id)
                .Association(x=>x.Category,x=>x.CategoryId,x=>x.Id)
                .Association(x=>x.CategoryOperationAccesses,x=>x.Id,x=>x.CategoryAccessId);

            mp.Entity<CategoryOperationAccess>()
                .HasTableName("CategoryOperationAccesses")
                .HasPrimaryKey(x => new {x.CategoryAccessId, x.OperationKeyId})
                .Association(x=>x.OperationKey,x=>x.OperationKeyId,x=>x.OperationKeyId);

            mp.Entity<Material>()
                .HasTableName("Materials")
                .HasIdentity(x => x.Id).HasPrimaryKey(x => x.Id)
                .Association(x => x.Messages, (x, y) => x.Id == y.MaterialId)
                .Association(x => x.TagMaterials, (x, y) => x.Id == y.MaterialId)
                .Association(x => x.Author, x => x.AuthorId, x => x.Id)
                .Association(x => x.LastMessage, x => x.LastMessageId, x => x.Id)
                .Association(x => x.Category, x => x.CategoryId, x => x.Id);

            mp.Entity<Message>()
                .HasTableName("Messages")
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
            
            mp.Entity<UserToGroup>()
                .HasTableName("AspNetUserRoles")
                .HasPrimaryKey(x => new {x.UserId, x.RoleId})
                .Association(x => x.User, x => x.UserId, x => x.Id)
                .Association(x => x.UserGroup, x => x.RoleId, x => x.Id);

            mp.Entity<LongSession>()
                .HasTableName("LongSessions")
                .HasIdentity(x => x.Id)
                .HasPrimaryKey(x => x.Id)
                .Association(x => x.User, x => x.UserId, x => x.Id);

        }
    }
}