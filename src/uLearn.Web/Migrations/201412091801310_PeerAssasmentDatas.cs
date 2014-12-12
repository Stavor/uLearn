namespace uLearn.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PeerAssasmentDatas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Answers",
                c => new
                    {
                        AnswerId = c.Int(nullable: false, identity: true),
                        PropositionId = c.Int(nullable: false),
                        UserId = c.String(),
                        CourseId = c.String(),
                        SlideId = c.String(),
                    })
                .PrimaryKey(t => t.AnswerId)
                .ForeignKey("dbo.Propositions", t => t.PropositionId, cascadeDelete: true)
                .Index(t => t.PropositionId);
            
            CreateTable(
                "dbo.Propositions",
                c => new
                    {
                        PropositionId = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PropositionId);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewId = c.Int(nullable: false, identity: true),
                        PropositionForReviewId = c.Int(nullable: false),
                        Text = c.String(),
                        PropositionForReview_PropositionId = c.Int(),
                        Answer_AnswerId = c.Int(),
                    })
                .PrimaryKey(t => t.ReviewId)
                .ForeignKey("dbo.Propositions", t => t.PropositionForReview_PropositionId)
                .ForeignKey("dbo.Answers", t => t.Answer_AnswerId)
                .Index(t => t.PropositionForReview_PropositionId)
                .Index(t => t.Answer_AnswerId);
            
            CreateTable(
                "dbo.Marks",
                c => new
                    {
                        MarkId = c.Int(nullable: false, identity: true),
                        RebviewId = c.Int(nullable: false),
                        Value = c.String(),
                        Criterion = c.String(),
                        Review_ReviewId = c.Int(),
                    })
                .PrimaryKey(t => t.MarkId)
                .ForeignKey("dbo.Reviews", t => t.Review_ReviewId)
                .Index(t => t.Review_ReviewId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "Answer_AnswerId", "dbo.Answers");
            DropForeignKey("dbo.Reviews", "PropositionForReview_PropositionId", "dbo.Propositions");
            DropForeignKey("dbo.Marks", "Review_ReviewId", "dbo.Reviews");
            DropForeignKey("dbo.Answers", "PropositionId", "dbo.Propositions");
            DropIndex("dbo.Marks", new[] { "Review_ReviewId" });
            DropIndex("dbo.Reviews", new[] { "Answer_AnswerId" });
            DropIndex("dbo.Reviews", new[] { "PropositionForReview_PropositionId" });
            DropIndex("dbo.Answers", new[] { "PropositionId" });
            DropTable("dbo.Marks");
            DropTable("dbo.Reviews");
            DropTable("dbo.Propositions");
            DropTable("dbo.Answers");
        }
    }
}
