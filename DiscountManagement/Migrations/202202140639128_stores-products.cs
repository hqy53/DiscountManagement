namespace DiscountManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class storesproducts : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        ProductID = c.Int(nullable: false, identity: true),
                        ProductName = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        URL = c.String(),
                    })
                .PrimaryKey(t => t.ProductID);
            
            CreateTable(
                "dbo.ProductStores",
                c => new
                    {
                        Product_ProductID = c.Int(nullable: false),
                        Store_StoreID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Product_ProductID, t.Store_StoreID })
                .ForeignKey("dbo.Products", t => t.Product_ProductID, cascadeDelete: true)
                .ForeignKey("dbo.Stores", t => t.Store_StoreID, cascadeDelete: true)
                .Index(t => t.Product_ProductID)
                .Index(t => t.Store_StoreID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductStores", "Store_StoreID", "dbo.Stores");
            DropForeignKey("dbo.ProductStores", "Product_ProductID", "dbo.Products");
            DropIndex("dbo.ProductStores", new[] { "Store_StoreID" });
            DropIndex("dbo.ProductStores", new[] { "Product_ProductID" });
            DropTable("dbo.ProductStores");
            DropTable("dbo.Products");
        }
    }
}
