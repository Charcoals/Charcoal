using FluentMigrator;

namespace Charcoal.Migrations
{
    [Migration(6)]
    public class AddEstimateAndStoryTypeAndIterationTypeToStoriesTable:Migration
    {
        public override void Up()
        {
            Alter.Table("Stories")
                .AddColumn("Estimate").AsInt32().Nullable()
                .AddColumn("StoryType").AsInt32().WithDefaultValue(0)
                .AddColumn("IterationType").AsInt32().WithDefaultValue(0);
        }

        public override void Down()
        {
            Delete.Column("Estimate").FromTable("Stories");
            Delete.Column("StoryType").FromTable("Stories");
            Delete.Column("IterationType").FromTable("Stories");
        }
    }
}