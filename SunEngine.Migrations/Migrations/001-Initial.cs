using System;
using System.Data;
using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    /// <summary>
    /// Initial migration for FluentMigrator
    /// </summary>
    [Migration(20190715000000)]
    public class Initial : Migration
    {
        public override void Up()
        {
            Create.Table("cache_settings")
                .WithColumn("id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("cache_policy").AsInt32().NotNullable()
                .WithColumn("invalidate_cache_time").AsInt32().Nullable();


            Create.Table("category_cache_settings")
                .WithColumn("id").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("pages_amount").AsInt32().NotNullable();


            Create.Table("categories")
                .WithColumn("id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("parent_id").AsInt32().Indexed().Nullable()
                .ForeignKey("categories", "id")
                .WithColumn("name").AsString(DbColumnSizes.Categories_Name).NotNullable()
                .WithColumn("name_normalized").AsString(DbColumnSizes.Categories_Name).NotNullable().Unique()
                .WithColumn("title").AsString(DbColumnSizes.Categories_Title).NotNullable()
                .WithColumn("sub_title").AsMaxString().Nullable()
                .WithColumn("icon").AsString(DbColumnSizes.Categories_Icon).Nullable()
                .WithColumn("material_type_title").AsString(DbColumnSizes.Categories_MaterialTypeTitle).Nullable()
                .WithColumn("header").AsMaxString().Nullable()
                .WithColumn("is_materials_container").AsBoolean().NotNullable()
                .WithColumn("is_materials_name_editable").AsBoolean().NotNullable().WithDefaultValue(false)
                .WithColumn("materials_sub_title_input_type").AsInt16().NotNullable().WithDefaultValue(0)
                .WithColumn("materials_preview_generator_name")
                .AsString(DbColumnSizes.Categories_MaterialsPreviewGeneratorName).Nullable()
                .WithColumn("cache_settings_id").AsInt32().Indexed().Nullable()
                .ForeignKey("category_cache_settings", "id")
                .WithColumn("sort_number").AsInt32().NotNullable().Unique()
                .WithColumn("layout_name").AsString(DbColumnSizes.Categories_LayoutName).Nullable()
                .WithColumn("settings_json").AsMaxString().Nullable()
                .WithColumn("is_cache_content").AsBoolean().NotNullable()
                .WithColumn("is_hidden").AsBoolean().NotNullable()
                .WithColumn("deleted_date").AsDateTime().Nullable();


            Create.Table("asp_net_users")
                .WithColumn("id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("user_name").AsString(DbColumnSizes.Users_UserName).NotNullable()
                .WithColumn("normalized_user_name").AsString(DbColumnSizes.Users_UserName).Unique().Nullable()
                .WithColumn("email").AsString(DbColumnSizes.Users_Email).Nullable()
                .WithColumn("normalized_email").AsString(DbColumnSizes.Users_Email).Unique().Nullable()
                .WithColumn("email_confirmed").AsBoolean().NotNullable()
                .WithColumn("password_hash").AsMaxString().Nullable()
                .WithColumn("security_stamp").AsMaxString().Nullable()
                .WithColumn("concurrency_stamp").AsMaxString().Nullable()
                .WithColumn("phone_number").AsMaxString().Nullable()
                .WithColumn("phone_number_confirmed").AsBoolean().NotNullable()
                .WithColumn("two_factor_enabled").AsBoolean().NotNullable()
                .WithColumn("lockout_end").AsDateTime().Nullable()
                .WithColumn("lockout_enabled").AsBoolean().NotNullable()
                .WithColumn("access_failed_count").AsInt16().NotNullable()
                .WithColumn("link").AsString(DbColumnSizes.Users_Link).Nullable()
                .WithColumn("information").AsMaxString().Nullable()
                .WithColumn("photo").AsString(DbColumnSizes.FileNameWithDirSize).Nullable()
                .WithColumn("avatar").AsString(DbColumnSizes.FileNameWithDirSize).Nullable()
                .WithColumn("registered_date").AsDateTime().Indexed();


            Create.Table("user_baned_unit")
                .WithColumn("user_id").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("asp_net_users", "id")
                .WithColumn("user_baned_id").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("asp_net_users", "id");


            Create.Table("materials")
                .WithColumn("id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("name").AsString(DbColumnSizes.Materials_Name).Nullable().Unique()
                .WithColumn("title").AsString(DbColumnSizes.Materials_Title).NotNullable()
                .WithColumn("sub_title").AsString(DbColumnSizes.Materials_SubTitle).Nullable()
                .WithColumn("preview").AsMaxString().Nullable()
                .WithColumn("text").AsMaxString().NotNullable()
                .WithColumn("category_id").AsInt32().NotNullable().Indexed()
                .ForeignKey("categories", "id")
                .WithColumn("author_id").AsInt32().NotNullable().Indexed()
                .ForeignKey("asp_net_users", "id")
                .WithColumn("publish_date").AsDateTime().NotNullable().Indexed()
                .WithColumn("edit_date").AsDateTime().Nullable()
                .WithColumn("last_comment_id").AsInt32().Nullable()
                .WithColumn("last_activity").AsDateTime().NotNullable().Indexed()
                .WithColumn("comments_count").AsInt32().NotNullable()
                .WithColumn("sort_number").AsInt32().NotNullable().Unique()
                .WithColumn("is_comments_blocked").AsBoolean().NotNullable()
                .WithColumn("is_hidden").AsBoolean().NotNullable()
                .WithColumn("deleted_date").AsDateTime().Nullable();


            Create.Table("comments")
                .WithColumn("id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("text").AsMaxString().NotNullable()
                .WithColumn("material_id").AsInt32().NotNullable().Indexed()
                .ForeignKey("materials", "id").OnDelete(Rule.Cascade)
                .WithColumn("author_id").AsInt32().Indexed().Nullable()
                .ForeignKey("asp_net_users", "id")
                .WithColumn("publish_date").AsDateTime().NotNullable().Indexed()
                .WithColumn("edit_date").AsDateTime().Nullable()
                .WithColumn("deleted_date").AsDateTime().Nullable();

            Create.ForeignKey().FromTable("materials")
                .ForeignColumn("last_comment_id").ToTable("comments").PrimaryColumn("id");


            Create.Table("tags")
                .WithColumn("id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("name").AsString(DbColumnSizes.Tags_Name).Unique().NotNullable()
                .WithColumn("group_id").AsInt32().Nullable().Indexed();


            Create.Table("tag_materials")
                .WithColumn("tag_id").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("tags", "id")
                .WithColumn("material_id").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("materials", "id");


            Create.Table("asp_net_roles")
                .WithColumn("id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("name").AsString(DbColumnSizes.Roles_Name).Nullable()
                .WithColumn("normalized_name").AsString(DbColumnSizes.Roles_Name).Unique().Nullable()
                .WithColumn("concurrency_stamp").AsMaxString().Nullable()
                .WithColumn("title").AsString(DbColumnSizes.Roles_Title).Nullable()
                .WithColumn("sort_number").AsInt32().NotNullable()
                .WithColumn("is_super").AsBoolean().NotNullable();


            Create.Table("asp_net_user_roles")
                .WithColumn("user_id").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("asp_net_users", "id")
                .WithColumn("role_id").AsInt32().PrimaryKey().NotNullable().Indexed()
                .ForeignKey("asp_net_roles", "id");


            Create.Table("operation_keys")
                .WithColumn("operation_key_id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("name").AsString(DbColumnSizes.OperationKey_Name).NotNullable().Indexed();


            Create.Table("category_accesses")
                .WithColumn("id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("role_шd").AsInt32().NotNullable().Indexed()
                .ForeignKey("asp_net_roles", "id")
                .WithColumn("category_id").AsInt32().NotNullable().Indexed()
                .ForeignKey("categories", "id");


            Create.Table("category_operation_accesses")
                .WithColumn("category_access_id").AsInt32().PrimaryKey().NotNullable().Indexed().ForeignKey(
                    "category_accesses", "id")
                .WithColumn("operation_key_id").AsInt32().PrimaryKey().NotNullable().Indexed().ForeignKey(
                    "operation_keys", "operation_key_id")
                .WithColumn("access").AsBoolean().NotNullable();


            Create.Table("long_sessions")
                .WithColumn("id").AsInt64().PrimaryKey().Identity().NotNullable()
                .WithColumn("user_id").AsInt32().NotNullable().Indexed()
                .ForeignKey("asp_net_users", "id")
                .WithColumn("long_token1").AsString(DbColumnSizes.LongSessions_LongToken1).NotNullable()
                .WithColumn("long_token2").AsString(DbColumnSizes.LongSessions_LongToken2).NotNullable()
                .WithColumn("device_info").AsMaxString().NotNullable()
                .WithColumn("update_Date").AsDateTime().NotNullable()
                .WithColumn("expiration_date").AsDateTime().NotNullable().Indexed();

            Create.Index().OnTable("long_sessions")
                .OnColumn("user_id").Ascending()
                .OnColumn("long_token1").Ascending()
                .OnColumn("long_token2").Ascending();


            Create.Table("black_list_short_tokens")
                .WithColumn("token_id").AsString(DbColumnSizes.BlackListShortToken_TokenId).PrimaryKey().NotNullable()
                .WithColumn("expire").AsDateTime().Indexed().NotNullable();


            Create.Table("menu_items")
                .WithColumn("id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("parent_id").AsInt32().Nullable()
                .ForeignKey("menu_items", "id")
                .WithColumn("name").AsString(DbColumnSizes.MenuItems_Name).Nullable()
                .WithColumn("title").AsString(DbColumnSizes.MenuItems_Title).NotNullable()
                .WithColumn("sub_title").AsString(DbColumnSizes.MenuItems_SubTitle).Nullable()
                .WithColumn("route_name").AsString(DbColumnSizes.MenuItems_RouteName).Nullable()
                .WithColumn("route_params_json").AsMaxString().Nullable()
                .WithColumn("exact").AsBoolean().NotNullable()
                .WithColumn("roles").AsMaxString().Nullable()
                .WithColumn("settings_json").AsMaxString().Nullable()
                .WithColumn("css_class").AsString(DbColumnSizes.MenuItems_CssClass).Nullable()
                .WithColumn("external_url").AsMaxString().Nullable()
                .WithColumn("is_separator").AsBoolean().NotNullable()
                .WithColumn("sort_number").AsInt32().NotNullable().Unique()
                .WithColumn("icon").AsString(DbColumnSizes.MenuItems_Icon).Nullable()
                .WithColumn("is_hidden").AsBoolean().NotNullable();

            Create.Table("cipher_secrets")
                .WithColumn("name").AsString(DbColumnSizes.CipherSecrets_Name).PrimaryKey().NotNullable()
                .WithColumn("secret").AsString(DbColumnSizes.CipherSecrets_Secret).NotNullable();
        }


        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
