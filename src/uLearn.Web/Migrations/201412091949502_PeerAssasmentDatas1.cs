namespace uLearn.Web.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PeerAssasmentDatas1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Propositions", "Timestamp", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Propositions", "Timestamp", c => c.DateTime(nullable: false));
        }
    }
}
