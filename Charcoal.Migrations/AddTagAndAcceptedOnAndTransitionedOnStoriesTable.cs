using FluentMigrator;

namespace Charcoal.Migrations
{
    [Migration(8)]
    public class AddTagAndAcceptedOnAndTransitionedOnStoriesTable:Migration
    {
        public override void Up()
        {
            Alter.Table("Stories")
                .AddColumn("Tag").AsString().Nullable()
                .AddColumn("AcceptedOn").AsDateTime().Nullable()
                .AddColumn("TransitionedOn").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Column("Tag").FromTable("Stories");
            Delete.Column("AcceptedOn").FromTable("Stories");
            Delete.Column("TransitionedOn").FromTable("Stories");
        }
    }
}