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
            Create.Table("CacheSettings".s())
                .WithColumn("Id".s()).AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("CachePolicy".s()).AsInt32().NotNullable()
                .WithColumn("InvalidateCacheTime".s()).AsInt32().Nullable();


            Create.Table("CategoryCacheSettings".s())
                .WithColumn("Id".s()).AsInt32().PrimaryKey().NotNullable()
                .WithColumn("PagesAmount".s()).AsInt32().NotNullable();


            Create.Table("Categories".s())
                .WithColumn("Id".s()).AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("ParentId".s()).AsInt32().Indexed().Nullable()
                .ForeignKey("fkCategoriesCategories".s(), "Categories".s(), "Id".s())
                .WithColumn("Name".s()).AsString(DbColumnSizes.Categories_Name).NotNullable()
                .WithColumn("NameNormalized".s()).AsString(DbColumnSizes.Categories_Name).NotNullable().Unique()
                .WithColumn("Title".s()).AsString(DbColumnSizes.Categories_Title).NotNullable()
                .WithColumn("SubTitle".s()).AsMaxString().Nullable()
                .WithColumn("Icon".s()).AsString(DbColumnSizes.Categories_Icon).Nullable()
                .WithColumn("MaterialTypeTitle".s()).AsString(DbColumnSizes.Categories_MaterialTypeTitle).Nullable()
                .WithColumn("Header".s()).AsMaxString().Nullable()
                .WithColumn("IsMaterialsContainer".s()).AsBoolean().NotNullable()
                .WithColumn("IsMaterialsNameEditable".s()).AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("MaterialsSubTitleInputType".s()).AsInt16().NotNullable().WithDefaultValue(0)
                .WithColumn("MaterialsPreviewGeneratorName".s())
                .AsString(DbColumnSizes.Categories_MaterialsPreviewGeneratorName).Nullable()
                .WithColumn("CacheSettingsId".s()).AsInt32().Indexed().Nullable()
                .ForeignKey("fkCategoriesCacheSettings".s(), "CategoryCacheSettings".s(), "Id".s())
                .WithColumn("SortNumber".s()).AsInt32().NotNullable().Unique()
                .WithColumn("LayoutName".s()).AsString(DbColumnSizes.Categories_LayoutName).Nullable()
                .WithColumn("SettingsJson".s()).AsMaxString().Nullable()
                .WithColumn("IsCacheContent".s()).AsBoolean().NotNullable()
                .WithColumn("IsHidden".s()).AsBoolean().NotNullable()
                .WithColumn("DeletedDate".s()).AsDateTime().Nullable();


            Create.Table("AspNetUsers".s())
                .WithColumn("Id".s()).AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("UserName".s()).AsString(DbColumnSizes.Users_UserName).NotNullable()
                .WithColumn("NormalizedUserName".s()).AsString(DbColumnSizes.Users_UserName).Unique().Nullable()
                .WithColumn("Email".s()).AsString(DbColumnSizes.Users_Email).Nullable()
                .WithColumn("NormalizedEmail".s()).AsString(DbColumnSizes.Users_Email).Unique().Nullable()
                .WithColumn("EmailConfirmed".s()).AsBoolean().NotNullable()
                .WithColumn("PasswordHash".s()).AsMaxString().Nullable()
                .WithColumn("SecurityStamp".s()).AsMaxString().Nullable()
                .WithColumn("ConcurrencyStamp".s()).AsMaxString().Nullable()
                .WithColumn("PhoneNumber".s()).AsMaxString().Nullable()
                .WithColumn("PhoneNumberConfirmed".s()).AsBoolean().NotNullable()
                .WithColumn("TwoFactorEnabled".s()).AsBoolean().NotNullable()
                .WithColumn("LockoutEnd".s()).AsDateTime().Nullable()
                .WithColumn("LockoutEnabled".s()).AsBoolean().NotNullable()
                .WithColumn("AccessFailedCount".s()).AsInt16().NotNullable()
                .WithColumn("Link".s()).AsString(DbColumnSizes.Users_Link).Nullable()
                .WithColumn("Information".s()).AsMaxString().Nullable()
                .WithColumn("Photo".s()).AsString(DbColumnSizes.FileNameWithDirSize).Nullable()
                .WithColumn("Avatar".s()).AsString(DbColumnSizes.FileNameWithDirSize).Nullable()
                .WithColumn("RegisteredDate".s()).AsDateTime().Indexed()
                .WithColumn("ProfileVisitsCount".s()).AsInt32().NotNullable();
            

            Create.Table("UserBanedUnit".s())
                .WithColumn("UserId".s()).AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("fkUserBanedUnitUsers".s(), "AspNetUsers".s(), "Id".s())
                .WithColumn("UserBanedId".s()).AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("fkUserBanedUnitUsersBaned".s(), "AspNetUsers".s(), "Id".s());


            Create.Table("Materials".s())
                .WithColumn("Id".s()).AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name".s()).AsString(DbColumnSizes.Materials_Name).Nullable().Unique()
                .WithColumn("Title".s()).AsString(DbColumnSizes.Materials_Title).NotNullable()
                .WithColumn("SubTitle".s()).AsString(DbColumnSizes.Materials_SubTitle).Nullable()
                .WithColumn("Preview".s()).AsMaxString().Nullable()
                .WithColumn("Text".s()).AsMaxString().NotNullable()
                .WithColumn("CategoryId".s()).AsInt32().NotNullable().Indexed()
                .ForeignKey("fkMaterialsCategories".s(), "Categories".s(), "Id".s())
                .WithColumn("AuthorId".s()).AsInt32().NotNullable().Indexed()
                .ForeignKey("fkMaterialsUsers".s(), "AspNetUsers".s(), "Id".s())
                .WithColumn("PublishDate".s()).AsDateTime().NotNullable().Indexed()
                .WithColumn("EditDate".s()).AsDateTime().Nullable()
                .WithColumn("LastCommentId".s()).AsInt32().Nullable()
                .WithColumn("LastActivity".s()).AsDateTime().NotNullable().Indexed()
                .WithColumn("CommentsCount".s()).AsInt32().NotNullable()
                .WithColumn("SortNumber".s()).AsInt32().NotNullable().Unique()
                .WithColumn("IsCommentsBlocked".s()).AsBoolean().NotNullable()
                .WithColumn("SettingsJson".s()).AsMaxString().Nullable()
                .WithColumn("VisitsCount".s()).AsInt32().NotNullable()
                .WithColumn("IsHidden".s()).AsBoolean().NotNullable()
                .WithColumn("DeletedDate".s()).AsDateTime().Nullable();


            Create.Table("Comments".s())
                .WithColumn("Id".s()).AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Text".s()).AsMaxString().NotNullable()
                .WithColumn("MaterialId".s()).AsInt32().NotNullable().Indexed()
                .ForeignKey("fkCommentsMaterials".s(), "Materials".s(), "Id".s()).OnDelete(Rule.Cascade)
                .WithColumn("AuthorId".s()).AsInt32().Indexed().Nullable()
                .ForeignKey("fkCommentsUsers".s(), "AspNetUsers".s(), "Id".s())
                .WithColumn("PublishDate".s()).AsDateTime().NotNullable().Indexed()
                .WithColumn("EditDate".s()).AsDateTime().Nullable()
                .WithColumn("DeletedDate".s()).AsDateTime().Nullable();

            Create.ForeignKey("fkMaterialsLastComment".s()).FromTable("Materials".s())
                .ForeignColumn("LastCommentId".s()).ToTable("Comments".s()).PrimaryColumn("Id".s());


            Create.Table("Tags".s())
                .WithColumn("Id".s()).AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name".s()).AsString(DbColumnSizes.Tags_Name).Unique().NotNullable()
                .WithColumn("GroupId".s()).AsInt32().Nullable().Indexed();


            Create.Table("TagMaterials".s())
                .WithColumn("TagId".s()).AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("fkTagMaterialsTags".s(), "Tags".s(), "Id".s())
                .WithColumn("MaterialId".s()).AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("fkTagMaterialsTagMaterials".s(), "Materials".s(), "Id".s());


            Create.Table("AspNetRoles".s())
                .WithColumn("Id".s()).AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name".s()).AsString(DbColumnSizes.Roles_Name).Nullable()
                .WithColumn("NormalizedName".s()).AsString(DbColumnSizes.Roles_Name).Unique().Nullable()
                .WithColumn("ConcurrencyStamp".s()).AsMaxString().Nullable()
                .WithColumn("Title".s()).AsString(DbColumnSizes.Roles_Title).Nullable()
                .WithColumn("SortNumber".s()).AsInt32().NotNullable()
                .WithColumn("IsSuper".s()).AsBoolean().NotNullable();


            Create.Table("AspNetUserRoles".s())
                .WithColumn("UserId".s()).AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("fkUserRolesUsers".s(), "AspNetUsers".s(), "Id".s())
                .WithColumn("RoleId".s()).AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("fkUserRolesRoles".s(), "AspNetRoles".s(), "Id".s());


            Create.Table("OperationKeys".s())
                .WithColumn("OperationKeyId".s()).AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("Name".s()).AsString(DbColumnSizes.OperationKey_Name).NotNullable().Indexed();


            Create.Table("CategoryAccesses".s())
                .WithColumn("Id".s()).AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("RoleId".s()).AsInt32().NotNullable().Indexed()
                .ForeignKey("fkCategoryAccessesRoles".s(), "AspNetRoles".s(), "Id".s())
                .WithColumn("CategoryId".s()).AsInt32().NotNullable().Indexed()
                .ForeignKey("fkCategoryAccessesRoleCategories".s(), "Categories".s(), "Id".s());


            Create.Table("CategoryOperationAccesses".s())
                .WithColumn("CategoryAccessId".s()).AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("fkCategoryOperationAccessesCategoryAccesses".s(), "CategoryAccesses".s(), "Id".s())
                .WithColumn("OperationKeyId".s()).AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("fkCategoryOperationAccessesOperationKeys".s(), "OperationKeys".s(), "OperationKeyId".s())
                .WithColumn("Access".s()).AsBoolean().NotNullable();


            Create.Table("LongSessions".s())
                .WithColumn("Id".s()).AsInt64().PrimaryKey().Identity().NotNullable()
                .WithColumn("UserId".s()).AsInt32().NotNullable().Indexed()
                .ForeignKey("fkLongSessionsUsers".s(), "AspNetUsers".s(), "Id".s())
                .WithColumn("LongToken1".s()).AsString(DbColumnSizes.LongSessions_LongToken1).NotNullable()
                .WithColumn("LongToken2".s()).AsString(DbColumnSizes.LongSessions_LongToken2).NotNullable()
                .WithColumn("DeviceInfo".s()).AsMaxString().NotNullable()
                .WithColumn("UpdateDate".s()).AsDateTime().NotNullable()
                .WithColumn("ExpirationDate".s()).AsDateTime().NotNullable().Indexed();

            Create.Index("ixLongSessionsUserLongToken1LongToken2").OnTable("LongSessions".s())
                .OnColumn("UserId".s()).Ascending()
                .OnColumn("LongToken1".s()).Ascending()
                .OnColumn("LongToken2".s()).Ascending();


            Create.Table("BlackListShortTokens".s())
                .WithColumn("TokenId".s()).AsString(DbColumnSizes.BlackListShortToken_TokenId).PrimaryKey()
                .NotNullable()
                .WithColumn("Expire".s()).AsDateTime().Indexed().NotNullable();


            Create.Table("MenuItems".s())
                .WithColumn("Id".s()).AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("ParentId".s()).AsInt32().Nullable()
                .ForeignKey("fkMenuItemsMenuItems".s(), "MenuItems".s(), "Id".s())
                .WithColumn("Name".s()).AsString(DbColumnSizes.MenuItems_Name).Nullable()
                .WithColumn("Title".s()).AsString(DbColumnSizes.MenuItems_Title).NotNullable()
                .WithColumn("SubTitle".s()).AsString(DbColumnSizes.MenuItems_SubTitle).Nullable()
                .WithColumn("RouteName".s()).AsString(DbColumnSizes.MenuItems_RouteName).Nullable()
                .WithColumn("RouteParamsJson".s()).AsMaxString().Nullable()
                .WithColumn("Exact".s()).AsBoolean().NotNullable()
                .WithColumn("Roles".s()).AsMaxString().Nullable()
                .WithColumn("SettingsJson".s()).AsMaxString().Nullable()
                .WithColumn("CssClass".s()).AsString(DbColumnSizes.MenuItems_CssClass).Nullable()
                .WithColumn("ExternalUrl".s()).AsMaxString().Nullable()
                .WithColumn("IsSeparator".s()).AsBoolean().NotNullable()
                .WithColumn("SortNumber".s()).AsInt32().NotNullable().Unique()
                .WithColumn("Icon".s()).AsString(DbColumnSizes.MenuItems_Icon).Nullable()
                .WithColumn("IsHidden".s()).AsBoolean().NotNullable();


            Create.Table("CipherSecrets".s())
                .WithColumn("Name".s()).AsString(DbColumnSizes.CipherSecrets_Name).PrimaryKey().NotNullable()
                .WithColumn("Secret".s()).AsString(DbColumnSizes.CipherSecrets_Secret).NotNullable();
        }


        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
