using System;
using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
	[Migration(20200107000000)]
	public class AddCategoryTokens : Migration
	{
		public override void Up()
		{
			Create.Column("Token".s()).OnTable("Categories".s())
				.AsString(DbColumnSizes.Categories_Token).Nullable();
			Create.Column("AppendTokenToSubCatsPath".s()).OnTable("Categories".s())
				.AsBoolean().NotNullable().WithDefaultValue(false);
			Create.Column("ShowInBreadcrumbs".s()).OnTable("Categories".s())
				.AsBoolean().NotNullable().WithDefaultValue(true);
		}

		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}