namespace DiscountManagement.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ProductDto : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ProductStores", newName: "StoreProducts");
            DropPrimaryKey("dbo.StoreProducts");
            AddPrimaryKey("dbo.StoreProducts", new[] { "Store_StoreID", "Product_ProductID" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.StoreProducts");
            AddPrimaryKey("dbo.StoreProducts", new[] { "Product_ProductID", "Store_StoreID" });
            RenameTable(name: "dbo.StoreProducts", newName: "ProductStores");
        }
    }
}
