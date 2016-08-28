namespace BeerPal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedSubscriptionToken : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subscriptions", "PayPalAgreementToken", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subscriptions", "PayPalAgreementToken");
        }
    }
}
