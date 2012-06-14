using FluentMigrator;

namespace Charcoal.Migrations
{
    [Migration(7)]
    public class AddVelocityToProjectTable : Migration
    {
        public override void Up()
        {
            Alter.Table("Projects")
                .AddColumn("Velocity")
                .AsInt32()
                .NotNullable()
                .WithDefaultValue(0);
        }

        public override void Down()
        {
            Delete.Column("Velocity")
                .FromTable("Projects");

        }
    }
}
