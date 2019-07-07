using System;
using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    [Migration(20190707000000)]
    public class AddLongSessionsUpdateDate : Migration
    {
        public override void Up()
        {
            DateTime dt = new DateTime(2019,07,07);
            Alter.Table("LongSessions").AddColumn("UpdateDate").AsDateTime().WithDefaultValue(dt).NotNullable();
        }


        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
