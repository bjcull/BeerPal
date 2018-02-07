namespace BeerPal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedWebHooks : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.WebhookEvents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PayPalWebHookEventId = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        DateReceived = c.DateTime(nullable: false),
                        EventType = c.String(),
                        Summary = c.String(),
                        ResourceType = c.String(),
                        ResourceJson = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.WebhookEvents");
        }
    }
}
