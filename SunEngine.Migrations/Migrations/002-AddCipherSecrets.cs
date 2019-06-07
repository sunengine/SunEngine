using FluentMigrator;

namespace SunEngine.Migrations.Migrations
{
    [Migration(20190606000000)]
    public class AddCipherSecrets  : Migration
    {
        public override void Up()
        {
            Create.Table("CipherSecrets")
                .WithColumn("Name").AsString(DbColumnSizes.CipherSecrets_Name).PrimaryKey().NotNullable()
                .WithColumn("Secret").AsString(DbColumnSizes.CipherSecrets_Secret).NotNullable();
        }

        public override void Down()
        {
            throw new System.NotImplementedException();
        }
    }
}
