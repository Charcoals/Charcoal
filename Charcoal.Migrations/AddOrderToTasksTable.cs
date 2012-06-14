using FluentMigrator;

namespace Charcoal.Migrations
{
    [Migration(9)]
    public class AddPositionToTasksTable : Migration
    {
        public override void Up()
        {
            Alter.Table("Tasks")
                .AddColumn("Position")
                .AsInt32()
                .NotNullable()
                .WithDefaultValue(0);
        }

        public override void Down()
        {
            Delete.Column("Position").FromTable("Tasks");
        }
    }
}