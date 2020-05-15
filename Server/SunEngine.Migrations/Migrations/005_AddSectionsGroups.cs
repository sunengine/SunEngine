using System;
using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
	/*[Migration(20200514000000)]
	public class AddSectionsGroups : Migration
	{
		public override void Up()
		{
			Delete.Table("Components".s());
			Create.Column("GroupName".s()).OnTable("Sections".s())
				.AsString(DbColumnSizes.Sections_GroupName).NotNullable()
				.SetExistingRowsTo("Pages");
		}

		public override void Down()
		{
			throw new NotImplementedException();
		}
	}*/
}