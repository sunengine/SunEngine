using System;
using System.Data;
using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    /// <summary>
    /// Initial migration for FluentMigrator
    /// </summary>
    [Migration(20190705000000)]
    public class Initial : Migration
    {
        public override void Up()
        {
            Create.Table("CacheSettings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("CachePolicy").AsInt32().NotNullable()
                .WithColumn("InvalidateCacheTime").AsInt32().Nullable();


            Create.Table("CategoryCacheSettings")
                .WithColumn("Id").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("PagesAmount").AsInt32().NotNullable();


            Create.Table("Categories")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("ParentId").AsInt32().Indexed().Nullable()
                .ForeignKey("FK_Categories_Categories_ParentId", "Categories", "Id")
                .WithColumn("Name").AsString(DbColumnSizes.Categories_Name).NotNullable()
                .WithColumn("NameNormalized").AsString(DbColumnSizes.Categories_Name).NotNullable().Unique()
                .WithColumn("Title").AsString(DbColumnSizes.Categories_Title).NotNullable()
                .WithColumn("SubTitle").AsMaxString().Nullable()
                .WithColumn("Icon").AsString(DbColumnSizes.Categories_Icon).Nullable()
                .WithColumn("MaterialTypeTitle").AsString(DbColumnSizes.Categories_MaterialTypeTitle).Nullable()
                .WithColumn("Header").AsMaxString().Nullable()
                .WithColumn("IsMaterialsContainer").AsBoolean().NotNullable()
                .WithColumn("IsMaterialsNameEditable").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("MaterialsSubTitleInputType").AsInt16().NotNullable().WithDefaultValue(0)
                .WithColumn("MaterialsPreviewGeneratorName").AsString(DbColumnSizes.Categories_MaterialsPreviewGeneratorName).Nullable()
                .WithColumn("CacheSettingsId").AsInt32().Indexed().Nullable()
                .ForeignKey("FK_Categories_CategoryCacheSettings_CacheSettingsId", "CategoryCacheSettings", "Id")
                .WithColumn("SortNumber").AsInt32().NotNullable().Unique()
                .WithColumn("LayoutName").AsString(DbColumnSizes.Categories_LayoutName).Nullable()
                .WithColumn("SettingsJson").AsMaxString().Nullable()
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
                .WithColumn("LockoutEnd").AsDateTime().Nullable()
                .WithColumn("LockoutEnabled").AsBoolean().NotNullable()
                .WithColumn("AccessFailedCount").AsInt16().NotNullable()
                .WithColumn("Link").AsString(DbColumnSizes.Users_Link).Nullable()
                .WithColumn("Information").AsMaxString().Nullable()
                .WithColumn("Photo").AsString(DbColumnSizes.FileNameWithDirSize).Nullable()
                .WithColumn("Avatar").AsString(DbColumnSizes.FileNameWithDirSize).Nullable()
                .WithColumn("RegisteredDate").AsDateTime().Indexed();


            Create.Table("UserBanedUnit")
                .WithColumn("UserId").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("FK_UserBanedUnit_AspNetUsers", "AspNetUsers", "Id")
                .WithColumn("UserBanedId").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("FK_UserBanedUnit_AspNetUsersBaned", "AspNetUsers", "Id");


            Create.Table("Materials")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name").AsString(DbColumnSizes.Materials_Name).Nullable().Unique()
                .WithColumn("Title").AsString(DbColumnSizes.Materials_Title).NotNullable()
                .WithColumn("SubTitle").AsString(DbColumnSizes.Materials_SubTitle).Nullable()
                .WithColumn("Preview").AsMaxString().Nullable()
                .WithColumn("Text").AsMaxString().NotNullable()
                .WithColumn("CategoryId").AsInt32().NotNullable().Indexed()
                .ForeignKey("FK_Materials_Categories_CategoryId", "Categories", "Id")
                .WithColumn("AuthorId").AsInt32().NotNullable().Indexed()
                .ForeignKey("FK_Materials_AspNetUsers_AuthorId", "AspNetUsers", "Id")
                .WithColumn("PublishDate").AsDateTime().NotNullable().Indexed()
                .WithColumn("EditDate").AsDateTime().Nullable()
                .WithColumn("LastCommentId").AsInt32().Nullable()
                .WithColumn("LastActivity").AsDateTime().NotNullable().Indexed()
                .WithColumn("CommentsCount").AsInt32().NotNullable()
                .WithColumn("SortNumber").AsInt32().NotNullable().Unique()
                .WithColumn("IsCommentsBlocked").AsBoolean().NotNullable()
                .WithColumn("IsHidden").AsBoolean().NotNullable()
                .WithColumn("IsDeleted").AsBoolean().NotNullable();


            Create.Table("Comments")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Text").AsMaxString().NotNullable()
                .WithColumn("MaterialId").AsInt32().NotNullable().Indexed()
                .ForeignKey("FK_Comments_Materials_MaterialId", "Materials", "Id").OnDelete(Rule.Cascade)
                .WithColumn("AuthorId").AsInt32().Indexed().Nullable()
                .ForeignKey("FK_Comments_AspNetUsers_AuthorId", "AspNetUsers", "Id")
                .WithColumn("PublishDate").AsDateTime().NotNullable().Indexed()
                .WithColumn("EditDate").AsDateTime().Nullable()
                .WithColumn("IsDeleted").AsBoolean().Nullable();

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
                .WithColumn("ExpirationDate").AsDateTime().NotNullable().Indexed();

            Create.Index("IX_LongSessions_Main").OnTable("LongSessions")
                .OnColumn("UserId").Ascending()
                .OnColumn("LongToken1").Ascending()
                .OnColumn("LongToken2").Ascending();


            Create.Table("BlackListShortTokens")
                .WithColumn("TokenId").AsString(DbColumnSizes.BlackListShortToken_TokenId).PrimaryKey().NotNullable()
                .WithColumn("Expire").AsDateTime().Indexed().NotNullable();


            Create.Table("MenuItems")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("ParentId").AsInt32().Nullable().ForeignKey("FK_MenuItems_MenuItems_Id", "MenuItems", "Id")
                .WithColumn("Name").AsString(DbColumnSizes.MenuItems_Name).Nullable()
                .WithColumn("Title").AsString(DbColumnSizes.MenuItems_Title).NotNullable()
                .WithColumn("SubTitle").AsString(DbColumnSizes.MenuItems_SubTitle).Nullable()
                .WithColumn("RouteName").AsString(DbColumnSizes.MenuItems_RouteName).Nullable()
                .WithColumn("RouteParamsJson").AsMaxString().Nullable()
                .WithColumn("Exact").AsBoolean().NotNullable()
                .WithColumn("Roles").AsMaxString().Nullable()
                .WithColumn("SettingsJson").AsMaxString().Nullable()
                .WithColumn("CssClass").AsString(DbColumnSizes.MenuItems_CssClass).Nullable()
                .WithColumn("ExternalUrl").AsMaxString().Nullable()
                .WithColumn("IsSeparator").AsBoolean().NotNullable()
                .WithColumn("SortNumber").AsInt32().NotNullable().Unique()
                .WithColumn("Icon").AsString(DbColumnSizes.MenuItems_Icon).Nullable()
                .WithColumn("IsHidden").AsBoolean().NotNullable();

            Create.Table("CipherSecrets")
                .WithColumn("Name").AsString(DbColumnSizes.CipherSecrets_Name).PrimaryKey().NotNullable()
                .WithColumn("Secret").AsString(DbColumnSizes.CipherSecrets_Secret).NotNullable();
        }


        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
