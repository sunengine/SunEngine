using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    [Migration(20191111000000)]
    public class RefactorIsMaterialsSubTitleEditable : Migration
    {
        public override void Up()
        {
            Delete.Column("MaterialsSubTitleInputType".s()).FromTable("Categories".s());
            Create.Column("IsMaterialsSubTitleEditable".s()).OnTable("Categories".s())
                .AsBoolean().NotNullable().SetExistingRowsTo(false);
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}