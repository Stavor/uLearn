namespace uLearn.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newMigratin : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Reviews", "PropositionForReview_PropositionId", "dbo.Propositions");
            DropIndex("dbo.Reviews", new[] { "PropositionForReview_PropositionId" });
            DropColumn("dbo.Reviews", "PropositionForReviewId");
            RenameColumn(table: "dbo.Reviews", name: "PropositionForReview_PropositionId", newName: "PropositionForReviewId");
            AlterColumn("dbo.Reviews", "PropositionForReviewId", c => c.Int(nullable: false));
            CreateIndex("dbo.Reviews", "PropositionForReviewId");
            AddForeignKey("dbo.Reviews", "PropositionForReviewId", "dbo.Propositions", "PropositionId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "PropositionForReviewId", "dbo.Propositions");
            DropIndex("dbo.Reviews", new[] { "PropositionForReviewId" });
            AlterColumn("dbo.Reviews", "PropositionForReviewId", c => c.Int());
            RenameColumn(table: "dbo.Reviews", name: "PropositionForReviewId", newName: "PropositionForReview_PropositionId");
            AddColumn("dbo.Reviews", "PropositionForReviewId", c => c.Int(nullable: false));
            CreateIndex("dbo.Reviews", "PropositionForReview_PropositionId");
            AddForeignKey("dbo.Reviews", "PropositionForReview_PropositionId", "dbo.Propositions", "PropositionId");
        }
    }
}
