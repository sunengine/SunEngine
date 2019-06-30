using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    [Migration(20190629000000)]
    public class MaterialsIsMaterialsSubTitleEditableChangeTypeAndName : Migration
    {
        public override void Up()
        {
            Delete.Column("IsMaterialsSubTitleEditable").FromTable("Categories");
            Create.Column("MaterialsSubTitleCreateType").OnTable("Categories").AsInt16().NotNullable().WithDefaultValue(0);
            Create.Column("MaterialsPreviewGeneratorName").OnTable("Categories").AsString(DbColumnSizes.Categories_MaterialsPreviewGeneratorName).Nullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}
