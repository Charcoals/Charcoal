using System.Data;
using FluentMigrator;

namespace Charcoal.Migrations
{
    [Migration(4)]
    public class CreateProjectTableAndAddProjectIdToStoriesTable : Migration
    {
        public override void Up()
        {
            Create.Table("Projects")
                .WithColumn("Id").AsInt64().PrimaryKey().Identity()
                .WithColumn("Title").AsString().NotNullable()
                .WithColumn("Description").AsString().Nullable();

            Alter.Table("Stories").AddColumn("ProjectId")
                .AsInt64().NotNullable().ForeignKey("ProjectId_To_Stories_FK", "Projects", "Id");

            Create.Table("UsersXProjects")
                .WithColumn("UserId").AsInt64()
                .ForeignKey("UserId_To_UsersXProjects_FK", "Users", "Id")
                .OnDeleteOrUpdate(Rule.Cascade)
                .NotNullable()
                .WithColumn("ProjectId").AsInt64()
                .ForeignKey("ProjectId_To_UsersXProjects_FK", "Projects", "Id")
                .OnDeleteOrUpdate(Rule.Cascade)
                .NotNullable();

            Create.UniqueConstraint("IX_UsersXProjects_Unique")
                .OnTable("UsersXProjects")
                .Columns(new[]{"UserId", "ProjectId"});
        }

        public override void Down()
        {
            Delete.ForeignKey("ProjectId_To_Stories_FK").OnTable("Stories");
            Delete.Column("ProjectId").FromTable("Stories");

            Delete.Table("UsersXProjects");
            Delete.Table("Projects");
        }
    }
}