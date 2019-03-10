using FluentMigrator;

namespace Migrations
{
    [Migration(20190310040300)]
    public class CacheContentMigration : Migration
    {
        public override void Up()
        {
            Alter.Table("Categories")
                .AddColumn("IsCacheContent")
                .AsBoolean()
                .NotNullable()
                .WithDefaultValue(false);
        }

        public override void Down()
        {
        }
    }
}