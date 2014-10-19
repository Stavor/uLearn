namespace uLearn.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_PeerAssasmentReview : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PeerAssasmentReviews",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PeerAssasmentAnswerId = c.Int(nullable: false),
                        CourseId = c.String(nullable: false, maxLength: 64),
                        SlideId = c.String(nullable: false, maxLength: 64),
                        UserId = c.String(maxLength: 64),
                        Text = c.String(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PeerAssasmentReviews");
        }
    }
}
