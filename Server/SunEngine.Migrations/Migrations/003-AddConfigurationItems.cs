using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    [Migration(20191108000000)]
    public class AddConfigurationItems : Migration
    {
        public override void Up()
        {
            Create.Table("ConfigurationItems".s())
                .WithColumn("Name".s()).AsString().PrimaryKey().NotNullable()
                .WithColumn("Value".s()).AsString().NotNullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}