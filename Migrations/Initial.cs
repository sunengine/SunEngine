using System;
using System.Data;
using System.Xml.Linq;
using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace Migrations
{
    [Migration(20190104130000)]
    public class Initial : Migration
    {
        public override void Up()
        {
            Create.Table("Categories")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name").AsString(256).NotNullable().Indexed()
                .WithColumn("Title").AsMaxString().NotNullable()
                .WithColumn("Description").AsMaxString().Nullable()
                .WithColumn("Header").AsMaxString().Nullable()
                .WithColumn("AreaRoot").AsBoolean().NotNullable()
                .WithColumn("IsMaterialsContainer").AsBoolean().NotNullable()
                .WithColumn("ParentId").AsInt32().Indexed().Nullable()
                .ForeignKey("FK_Categories_Categories_ParentId", "Categories", "Id")
                .WithColumn("SortNumber").AsInt32().NotNullable().Indexed()
                .WithColumn("IsHidden").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable();


            Create.Table("AspNetUsers")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("UserName").AsString(256).Nullable()
                .WithColumn("NormalizedUserName").AsString(256).Unique().Nullable()
                .WithColumn("Email").AsString(256).Nullable()
                .WithColumn("NormalizedEmail").AsString(256).Unique().Nullable()
                .WithColumn("EmailConfirmed").AsBoolean().NotNullable()
                .WithColumn("PasswordHash").AsMaxString().Nullable()
                .WithColumn("SecurityStamp").AsMaxString().Nullable()
                .WithColumn("ConcurrencyStamp").AsMaxString().Nullable()
                .WithColumn("PhoneNumber").AsMaxString().Nullable()
                .WithColumn("PhoneNumberConfirmed").AsBoolean().NotNullable()
                .WithColumn("TwoFactorEnabled").AsBoolean().NotNullable()
                .WithColumn("LockoutEnd").AsMyDateTime().Nullable()
                .WithColumn("LockoutEnabled").AsBoolean().NotNullable()
                .WithColumn("AccessFailedCount").AsInt16().NotNullable()
                .WithColumn("Link").AsMaxString().Nullable()
                .WithColumn("Information").AsMaxString().Nullable()
                .WithColumn("Photo").AsMaxString().Nullable()
                .WithColumn("Avatar").AsMaxString().Nullable();


            Create.Table("UserBanedUnit")
                .WithColumn("UserId").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("FK_UserBanedUnit_AspNetUsers", "AspNetUsers", "Id")
                .WithColumn("UserBanedId").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("FK_UserBanedUnit_AspNetUsersBaned", "AspNetUsers", "Id");


            Create.Table("Materials")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name").AsString(16).Nullable().Indexed()
                .WithColumn("Title").AsMaxString().NotNullable()
                .WithColumn("Description").AsMaxString().Nullable()
                .WithColumn("Preview").AsMaxString().Nullable()
                .WithColumn("Text").AsMaxString().NotNullable()
                .WithColumn("CategoryId").AsInt32().NotNullable().Indexed()
                .ForeignKey("FK_Materials_Categories_CategoryId", "Categories", "Id")
                .WithColumn("AuthorId").AsInt32().NotNullable().Indexed()
                .ForeignKey("FK_Materials_AspNetUsers_AuthorId", "AspNetUsers", "Id")
                .WithColumn("PublishDate").AsMyDateTime().NotNullable().Indexed()
                .WithColumn("EditDate").AsMyDateTime().Nullable()
                .WithColumn("LastMessageId").AsInt32().Nullable()
                .WithColumn("LastActivity").AsMyDateTime().NotNullable().Indexed()
                .WithColumn("MessagesCount").AsInt32().NotNullable()
                .WithColumn("SortNumber").AsInt32().NotNullable().Indexed()
                .WithColumn("IsDeleted").AsBoolean().NotNullable().Indexed();


            Create.Table("Messages")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Text").AsMaxString().NotNullable()
                .WithColumn("MaterialId").AsInt32().NotNullable().Indexed()
                .ForeignKey("FK_Messages_Materials_MaterialId", "Materials", "Id")
                .WithColumn("AuthorId").AsInt32().Indexed().Nullable()
                .ForeignKey("FK_Messages_AspNetUsers_AuthorId", "AspNetUsers", "Id")
                .WithColumn("PublishDate").AsMyDateTime().NotNullable().Indexed()
                .WithColumn("EditDate").AsMyDateTime().Nullable()
                .WithColumn("IsDeleted").AsBoolean().Nullable().Indexed();

            Create.ForeignKey("FK_Materials_Messages_LastMessageId").FromTable("Materials")
                .ForeignColumn("LastMessageId").ToTable("Messages").PrimaryColumn("Id");


            Create.Table("Tags")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name").AsString(256).Unique().NotNullable()
                .WithColumn("GroupId").AsInt32().Nullable().Indexed();


            Create.Table("TagMaterials")
                .WithColumn("TagId").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("FK_TagMaterials_Materials_MaterialId", "Tags", "Id")
                .WithColumn("MaterialId").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("FK_TagMaterials_Tags_TagId", "Materials", "Id");


            Create.Table("AspNetRoles")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name").AsString(256).Nullable()
                .WithColumn("NormalizedName").AsString(256).Unique().Nullable()
                .WithColumn("ConcurrencyStamp").AsMaxString().Nullable()
                .WithColumn("Title").AsMaxString().Nullable()
                .WithColumn("SortNumber").AsInt32().NotNullable()
                .WithColumn("IsSuper").AsBoolean().NotNullable();


            Create.Table("AspNetUserRoles")
                .WithColumn("UserId").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("FK_AspNetUserRoles_AspNetUsers_UserId", "AspNetUsers", "Id")
                .WithColumn("RoleId").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("FK_AspNetUserRoles_AspNetRoles_RoleId", "AspNetRoles", "Id");


            Create.Table("OperationKeys")
                .WithColumn("OperationKeyId").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name").AsString(256).NotNullable().Indexed();


            Create.Table("CategoryAccesses")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("UserGroupId").AsInt32().NotNullable().Indexed()
                .ForeignKey("FK_CategoryAccesses_AspNetRoles_UserGroupId", "AspNetRoles", "Id")
                .WithColumn("CategoryId").AsInt32().NotNullable().Indexed()
                .ForeignKey("FK_CategoryAccesses_Categories_CategoryId", "Categories", "Id");


            Create.Table("CategoryOperationAccesses")
                .WithColumn("CategoryAccessId").AsInt32().PrimaryKey().NotNullable().Indexed().ForeignKey(
                    "FK_CategoryOperationAccesses_CategoryAccesses_CategoryAccessId", "CategoryAccesses", "Id")
                .WithColumn("OperationKeyId").AsInt32().PrimaryKey().NotNullable().Indexed().ForeignKey(
                    "FK_CategoryOperationAccesses_OperationKeys_OperationKeyId", "OperationKeys", "OperationKeyId")
                .WithColumn("Access").AsBoolean().NotNullable();

            Create.Table("LongSessions")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity().NotNullable()
                .WithColumn("UserId").AsInt32().NotNullable().Indexed()
                .ForeignKey("FK_LongSession_AspNetUsers_Id", "AspNetUsers", "Id")
                .WithColumn("LongToken1").AsString(16).NotNullable()
                .WithColumn("LongToken2").AsString(16).NotNullable()
                .WithColumn("DeviceInfo").AsMaxString().NotNullable()
                .WithColumn("ExpirationDate").AsMyDateTime().NotNullable().Indexed();

            Create.Index("IX_LongSessions_Main").OnTable("LongSessions")
                .OnColumn("UserId").Ascending()
                .OnColumn("LongToken1").Ascending()
                .OnColumn("LongToken2").Ascending();
        }


        public override void Down()
        {
            throw new NotImplementedException();
        }
    }

    internal static class MigratorExtensions
    {
        public static ICreateTableColumnOptionOrWithColumnSyntax AsMaxString(
            this ICreateTableColumnAsTypeSyntax createTableColumnAsTypeSyntax)
        {
            return createTableColumnAsTypeSyntax.AsString(Int32.MaxValue);
        }

        public static ICreateTableColumnOptionOrWithColumnSyntax AsMyDateTime(
            this ICreateTableColumnAsTypeSyntax createTableColumnAsTypeSyntax)
        {
            if (!DBProvider.IsPostgre)
                return createTableColumnAsTypeSyntax.AsDateTime();
            else
                return createTableColumnAsTypeSyntax.AsCustom("TimestampTz");
        }
    }
}