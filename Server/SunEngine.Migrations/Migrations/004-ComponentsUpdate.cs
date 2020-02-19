using System;
using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
	[Migration(20200219000000)]
	public class SectionsUpdate : Migration
	{
		public override void Up()
		{
			Delete.Table("Sections".s());
			Create.Table("Sections".s())
				.WithColumn("Id".s()).AsInt32().PrimaryKey().Identity().NotNullable()
				.WithColumn("Name".s()).AsString(DbColumnSizes.Sections_Name).NotNullable().Unique()
				.WithColumn("Token".s()).AsString(DbColumnSizes.Sections_Token).Nullable()
				.WithColumn("Type".s()).AsString(DbColumnSizes.Sections_Type).NotNullable()
				.WithColumn("Roles".s()).AsString().NotNullable()
				.WithColumn("IsCacheData".s()).AsBoolean().NotNullable()
				.WithColumn("Options".s()).AsMaxString().NotNullable();
			//Delete.Column("Token".s()).FromTable("Categories".s());
		}

		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}