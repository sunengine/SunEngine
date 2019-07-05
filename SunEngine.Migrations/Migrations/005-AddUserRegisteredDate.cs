using System;
using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    [Migration(20190617000000)]
    public class AddUserRegisteredDate : Migration
    {
        public override void Up()
        {
            Create.Column("RegisteredDate").OnTable("AspNetUsers").AsDateTime().Indexed();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}
