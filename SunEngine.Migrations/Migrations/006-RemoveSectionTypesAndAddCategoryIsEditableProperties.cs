using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    [Migration(20190621000000)]
    public class RemoveSectionTypesAndAddCategoryIsEditableProperties : Migration
    {
        public override void Up()
        {
            Delete.Table("SectionTypes");
            Delete.Column("SectionTypeId").FromTable("Categories");
            Create.Column("IsMaterialsSubTitleEditable").OnTable("Categories").AsBoolean().NotNullable().WithDefaultValue(false);
            Create.Column("IsMaterialsNameEditable").OnTable("Categories").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}
