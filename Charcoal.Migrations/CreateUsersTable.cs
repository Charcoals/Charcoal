using FluentMigrator;

namespace Charcoal.Migrations
{
    [Migration(1)]
    public class CreateUsersTable : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("UserName").AsString().NotNullable()
                .WithColumn("FirstName").AsString().NotNullable()
                .WithColumn("LastName").AsString().NotNullable()
                .WithColumn("APIKey").AsString().Nullable()
                .WithColumn("Email").AsString().NotNullable()
                .WithColumn("Privileges").AsInt32().WithDefaultValue(0);
        }

        public override void Down()
        {
            Delete.Table("Users");
        }
    }
}