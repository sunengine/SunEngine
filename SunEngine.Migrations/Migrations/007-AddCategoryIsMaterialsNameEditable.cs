using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    [Migration(20190622000000)]
    public class AddCategoryIsMaterialsNameEditable : Migration
    {
        public override void Up()
        {
            Create.Column("IsMaterialsNameEditable").OnTable("Categories").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}
