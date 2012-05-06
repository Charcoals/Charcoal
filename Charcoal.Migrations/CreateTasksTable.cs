using FluentMigrator;

namespace Charcoal.Migrations
{
    [Migration(3)]
    public class CreateTasksTable:Migration
    {
        public override void Up()
        {
            Create.Table("Tasks")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Description").AsString().NotNullable()
                .WithColumn("Assignees").AsString().Nullable()
                .WithColumn("StoryId").AsInt64().ForeignKey("Stories_FK","Stories","Id")
                .WithColumn("IsCompleted").AsBoolean().WithDefaultValue(false)
                .WithColumn("CreatedOn").AsDateTime().NotNullable()
                .WithColumn("LastEditedOn").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Tasks");
        }
    }
}
