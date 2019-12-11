using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    [Migration(20190711000000)]
    public class AddFulltextSearchInMaterials : Migration
    {
        public override void Up()
        {
            IfDatabase("MySql5", "MySql", "MariaDB")
                .Execute.Sql("ALTER TABLE `Materials` ADD FULLTEXT INDEX `TFS` (`Title`, `Text`);");
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}
