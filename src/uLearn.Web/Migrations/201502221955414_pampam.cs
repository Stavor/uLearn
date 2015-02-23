namespace uLearn.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class pampam : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Marks", "RebviewId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Marks", "RebviewId", c => c.Int(nullable: false));
        }
    }
}
