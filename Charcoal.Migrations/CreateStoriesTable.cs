using FluentMigrator;

namespace Charcoal.Migrations
{
    [Migration(2)]
    public class CreateStoriesTable:Migration{
        public override void Up()
        {
            Create.Table("Stories")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Title").AsString().NotNullable()
                .WithColumn("Description").AsString().Nullable()
                .WithColumn("Status").AsInt32().WithDefaultValue(0)
                .WithColumn("CreatedBy").AsInt64().NotNullable().ForeignKey("Creator_FK","Users", "Id")
                .WithColumn("CreatedOn").AsDateTime().NotNullable()
                .WithColumn("LastEditedOn").AsDateTime().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Stories");
        }
    }
}