using System;
using FluentMigrator;
using FluentMigrator.Expressions;

namespace SunEngine.Migrations.Migrations
{
	[Migration(20200219000000)]
	public class ComponentsUpdate : Migration
	{
		public override void Up()
		{
			Delete.Table("Components".s());
			Create.Table("Components".s())
				.WithColumn("Id".s()).AsInt32().PrimaryKey().Identity().NotNullable()
				.WithColumn("Name".s()).AsString(DbColumnSizes.Components_Name).NotNullable().Unique()
				.WithColumn("Token".s()).AsString(DbColumnSizes.Components_Token).Nullable()
				.WithColumn("Type".s()).AsString(DbColumnSizes.Components_Type).NotNullable()
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