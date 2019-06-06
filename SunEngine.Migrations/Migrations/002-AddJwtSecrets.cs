using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    [Migration(20190606000000)]
    public class AddJwtSecrets  : Migration
    {
        public override void Up()
        {
            Create.Table("JwtSecrets")
                .WithColumn("Name").AsString(DbColumnSizes.JwtSecrets_Name).PrimaryKey().NotNullable()
                .WithColumn("Secret").AsString(DbColumnSizes.JwtSecrets_Secret).NotNullable();

        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}
