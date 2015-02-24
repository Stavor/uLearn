namespace uLearn.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class trashmigr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "WasSubmit", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Reviews", "WasSubmit");
        }
    }
}
