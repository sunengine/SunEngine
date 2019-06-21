using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    [Migration(20190621000000)]
    public class RemoveSectionTypesAndAddCategoryIsMaterialsDescriptionEditable : Migration
    {
        public override void Up()
        {
            Delete.Table("SectionTypes");
            Delete.Column("SectionTypeId").FromTable("Categories");
            Create.Column("IsMaterialsDescriptionEditable").OnTable("Categories").AsBoolean().NotNullable().WithDefaultValue(false);
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}
