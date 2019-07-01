using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    [Migration(20190627000000)]
    public class RenameColumnMaterialDescriptionToSubTitle : Migration
    {
        public override void Up()
        {
            Rename.Column("Description").OnTable("Materials").To("SubTitle");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}
