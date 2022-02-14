namespace DiscountManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DiscountCodeAndPercentage : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stores", "DiscountCode", c => c.String());
            AddColumn("dbo.Stores", "DiscountPercentage", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stores", "DiscountPercentage");
            DropColumn("dbo.Stores", "DiscountCode");
        }
    }
}
