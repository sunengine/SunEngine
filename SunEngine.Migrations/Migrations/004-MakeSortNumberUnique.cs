using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    [Migration(20190613000000)]
    public class MakeSortNumberUnique: Migration
    {
        public override void Up()
        {
            Delete.Index().OnTable("Materials").OnColumn("SortNumber");
            
            Alter.Column("SortNumber").OnTable("Materials").AsInt32().Unique().NotNullable();
            
            Alter.Column("SortNumber").OnTable("Categories").AsInt32().Unique().NotNullable();
            
            Alter.Column("SortNumber").OnTable("MenuItems").AsInt32().Unique().NotNullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}
