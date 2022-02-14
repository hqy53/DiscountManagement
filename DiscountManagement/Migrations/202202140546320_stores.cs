namespace DiscountManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class stores : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        StoreID = c.Int(nullable: false, identity: true),
                        StoreName = c.String(),
                    })
                .PrimaryKey(t => t.StoreID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Stores");
        }
    }
}
