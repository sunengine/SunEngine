using System;
using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
	[Migration(20200123000000)]
	public class MakeUserLinkIndexed : Migration
	{
		public override void Up()
		{
			Alter.Column("Link".s()).OnTable("AspNetUsers".s()).AsString(DbColumnSizes.Users_Link).Indexed().Nullable();
		}

		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}