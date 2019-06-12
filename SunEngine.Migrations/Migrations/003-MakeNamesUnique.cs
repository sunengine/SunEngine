using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    [Migration(20190612000000)]
    public class MakeNamesUnique: Migration
    {
        public override void Up()
        {
            Alter.Column("NameNormalized").OnTable("Categories").AsString(DbColumnSizes.Categories_Name).Unique().NotNullable();
            
            Delete.Index().OnTable("Materials").OnColumn("Name");
            Alter.Column("Name").OnTable("Materials").AsString(DbColumnSizes.Materials_Name).Unique().Nullable();
            
            Alter.Column("Name").OnTable("MenuItems").AsString(DbColumnSizes.MenuItems_Name).Unique().Nullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}
