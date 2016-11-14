namespace BeerPal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedShippingAndTax : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Tax", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Subtotal", c => c.Int(nullable: false));
            AddColumn("dbo.Orders", "Shipping", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Shipping");
            DropColumn("dbo.Orders", "Subtotal");
            DropColumn("dbo.Orders", "Tax");
        }
    }
}
