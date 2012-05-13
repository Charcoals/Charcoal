using FluentMigrator;

namespace Charcoal.Migrations
{
    [Migration(5)]
    public class AddPasswordColumnToUsersTableAndMakeEmailUnique : Migration
    {
        public override void Up()
        {
            Alter.Table("Users").AddColumn("Password").AsString().NotNullable();
            Create.UniqueConstraint("IX_Email_Users").OnTable("Users").Column("Email");
        }

        public override void Down()
        {
            Delete.Column("Password").FromTable("Users");
            Delete.UniqueConstraint("IX_Email_Users").FromTable("Users");
        }
    }
}