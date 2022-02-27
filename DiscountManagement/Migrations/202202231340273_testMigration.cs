namespace DiscountManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Stores", "StoreHasPic", c => c.Boolean(nullable: false));
            AddColumn("dbo.Stores", "PicExtension", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Stores", "PicExtension");
            DropColumn("dbo.Stores", "StoreHasPic");
        }
    }
}
