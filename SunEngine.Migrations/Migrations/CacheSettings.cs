using System;
using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    [Migration(20190402000000)]
    public class CacheSettings : Migration
    {
        public override void Up()
        {
            Create.Table("CacheSettings")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity().NotNullable()
                .WithColumn("CachePolicy").AsInt32().NotNullable();
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}