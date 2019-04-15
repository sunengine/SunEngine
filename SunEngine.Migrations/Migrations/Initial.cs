using System;
using System.Data;
using FluentMigrator;
using FluentMigrator.Builders.Create.Table;

namespace SunEngine.Migrations.Migrations
{
    /// <summary>
    /// Initial migration for FluentMigrator
    /// </summary>
    [Migration(20190208000000)]
    public class Initial : Migration
    {
        public override void Up()
        {
            
            Create.Table("SectionTypes")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name").AsString(DbColumnSizes.SectionType_Name).NotNullable()
                .WithColumn("Title").AsString(DbColumnSizes.SectionType_Title).NotNullable();

            Create.Table("Categories")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name").AsString(DbColumnSizes.Categories_Name).NotNullable()
                .WithColumn("NameNormalized").AsString(DbColumnSizes.Categories_Name).NotNullable()
                .WithColumn("Title").AsString(DbColumnSizes.Categories_Title).NotNullable()
                .WithColumn("MaterialTypeTitle").AsString(DbColumnSizes.Categories_MaterialTypeTitle).Nullable()
                .WithColumn("Description").AsMaxString().Nullable()
                .WithColumn("Header").AsMaxString().Nullable()
                .WithColumn("AppendUrlToken").AsBoolean().NotNullable()
                .WithColumn("SectionTypeId").AsInt32().Nullable()
                .WithColumn("IsMaterialsContainer").AsBoolean().NotNullable()
                .WithColumn("ParentId").AsInt32().Indexed().Nullable()
                .ForeignKey("FK_Categories_Categories_ParentId", "Categories", "Id")
                .WithColumn("SortNumber").AsInt32().NotNullable()
                .WithColumn("IsCacheContent").AsBoolean().NotNullable()
                .WithColumn("IsHidden").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable();


            Create.Table("AspNetUsers")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("UserName").AsString(DbColumnSizes.Users_UserName).NotNullable()
                .WithColumn("NormalizedUserName").AsString(DbColumnSizes.Users_UserName).Unique().Nullable()
                .WithColumn("Email").AsString(DbColumnSizes.Users_Email).Nullable()
                .WithColumn("NormalizedEmail").AsString(DbColumnSizes.Users_Email).Unique().Nullable()
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
                .WithColumn("Link").AsString(DbColumnSizes.Users_Link).Nullable()
                .WithColumn("Information").AsMaxString().Nullable()
                .WithColumn("Photo").AsString(DbColumnSizes.FileNameWithDirSize).Nullable()
                .WithColumn("Avatar").AsString(DbColumnSizes.FileNameWithDirSize).Nullable();


            Create.Table("UserBanedUnit")
                .WithColumn("UserId").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("FK_UserBanedUnit_AspNetUsers", "AspNetUsers", "Id")
                .WithColumn("UserBanedId").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("FK_UserBanedUnit_AspNetUsersBaned", "AspNetUsers", "Id");


            Create.Table("Materials")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name").AsString(DbColumnSizes.Materials_Name).Nullable().Indexed()
                .WithColumn("Title").AsString(DbColumnSizes.Materials_Title).NotNullable()
                .WithColumn("Description").AsMaxString().Nullable()
                .WithColumn("Preview").AsMaxString().Nullable()
                .WithColumn("Text").AsMaxString().NotNullable()
                .WithColumn("CategoryId").AsInt32().NotNullable().Indexed()
                .ForeignKey("FK_Materials_Categories_CategoryId", "Categories", "Id")
                .WithColumn("AuthorId").AsInt32().NotNullable().Indexed()
                .ForeignKey("FK_Materials_AspNetUsers_AuthorId", "AspNetUsers", "Id")
                .WithColumn("PublishDate").AsMyDateTime().NotNullable().Indexed()
                .WithColumn("EditDate").AsMyDateTime().Nullable()
                .WithColumn("LastCommentId").AsInt32().Nullable()
                .WithColumn("LastActivity").AsMyDateTime().NotNullable().Indexed()
                .WithColumn("CommentsCount").AsInt32().NotNullable()
                .WithColumn("SortNumber").AsInt32().NotNullable().Indexed()
                .WithColumn("IsHidden").AsBoolean().NotNullable().Indexed()
                .WithColumn("IsDeleted").AsBoolean().NotNullable().Indexed();


            Create.Table("Comments")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Text").AsMaxString().NotNullable()
                .WithColumn("MaterialId").AsInt32().NotNullable().Indexed()
                .ForeignKey("FK_Comments_Materials_MaterialId", "Materials", "Id").OnDelete(Rule.Cascade)
                .WithColumn("AuthorId").AsInt32().Indexed().Nullable()
                .ForeignKey("FK_Comments_AspNetUsers_AuthorId", "AspNetUsers", "Id")
                .WithColumn("PublishDate").AsMyDateTime().NotNullable().Indexed()
                .WithColumn("EditDate").AsMyDateTime().Nullable()
                .WithColumn("IsDeleted").AsBoolean().Nullable().Indexed();

            Create.ForeignKey("FK_Materials_Comments_LastCommentId").FromTable("Materials")
                .ForeignColumn("LastCommentId").ToTable("Comments").PrimaryColumn("Id");


            Create.Table("Tags")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name").AsString(DbColumnSizes.Tags_Name).Unique().NotNullable()
                .WithColumn("GroupId").AsInt32().Nullable().Indexed();


            Create.Table("TagMaterials")
                .WithColumn("TagId").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("FK_TagMaterials_Materials_MaterialId", "Tags", "Id")
                .WithColumn("MaterialId").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("FK_TagMaterials_Tags_TagId", "Materials", "Id");


            Create.Table("AspNetRoles")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name").AsString(DbColumnSizes.Roles_Name).Nullable()
                .WithColumn("NormalizedName").AsString(DbColumnSizes.Roles_Name).Unique().Nullable()
                .WithColumn("ConcurrencyStamp").AsMaxString().Nullable()
                .WithColumn("Title").AsString(DbColumnSizes.Roles_Title).Nullable()
                .WithColumn("SortNumber").AsInt32().NotNullable()
                .WithColumn("IsSuper").AsBoolean().NotNullable();


            Create.Table("AspNetUserRoles")
                .WithColumn("UserId").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("FK_AspNetUserRoles_AspNetUsers_UserId", "AspNetUsers", "Id")
                .WithColumn("RoleId").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("FK_AspNetUserRoles_AspNetRoles_RoleId", "AspNetRoles", "Id");


            Create.Table("OperationKeys")
                .WithColumn("OperationKeyId").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name").AsString(DbColumnSizes.OperationKey_Name).NotNullable().Indexed();


            Create.Table("CategoryAccesses")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("RoleId").AsInt32().NotNullable().Indexed()
                .ForeignKey("FK_CategoryAccesses_AspNetRoles_RoleId", "AspNetRoles", "Id")
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
                .WithColumn("LongToken1").AsString(DbColumnSizes.LongSessions_LongToken1).NotNullable()
                .WithColumn("LongToken2").AsString(DbColumnSizes.LongSessions_LongToken2).NotNullable()
                .WithColumn("DeviceInfo").AsMaxString().NotNullable()
                .WithColumn("ExpirationDate").AsMyDateTime().NotNullable().Indexed();

            Create.Index("IX_LongSessions_Main").OnTable("LongSessions")
                .OnColumn("UserId").Ascending()
                .OnColumn("LongToken1").Ascending()
                .OnColumn("LongToken2").Ascending();

            Create.Table("BlackListShortTokens")
                .WithColumn("TokenId").AsString(DbColumnSizes.BlackListShortToken_TokenId).PrimaryKey().NotNullable()
                .WithColumn("Expire").AsMyDateTime().Indexed().NotNullable();
        }


        public override void Down()
        {
            throw new NotImplementedException();
        }
    }

    internal static class DbColumnSizes
    {
        public const int SectionType_Name = 32;
        public const int SectionType_Title = 64;
        public const int Categories_Name = 64;
        public const int Categories_Title = 256;
        public const int Categories_MaterialTypeTitle = 32;
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
            if (!DbProvider.IsPostgre)
                return createTableColumnAsTypeSyntax.AsDateTime();

            return createTableColumnAsTypeSyntax.AsCustom("TimestampTz");
        }
    }
}