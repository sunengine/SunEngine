using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    [Migration(20190403000000)]
    public class CategoryCacheSettings : Migration
    {
        public override void Up()
        {
            Create.Table("CategoryCacheSettings")
                .WithColumn("Id").AsInt32().PrimaryKey().NotNullable()
                .WithColumn("PagesAmount").AsInt32().NotNullable();

            Alter.Table("Categories").AddColumn("CacheSettingsId").AsInt32().Indexed().Nullable()
                .ForeignKey("FK_Categories_CategoryCacheSettings_CacheSettingsId", "CategoryCacheSettings", "Id");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}