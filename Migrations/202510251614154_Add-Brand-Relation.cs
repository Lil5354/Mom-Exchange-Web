namespace B_M.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddBrandRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Brands", "UserId", c => c.Int());
            AddColumn("dbo.Products", "IsActive", c => c.Boolean(nullable: false));
            CreateIndex("dbo.Brands", "UserId");
            AddForeignKey("dbo.Brands", "UserId", "dbo.Users", "UserID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Brands", "UserId", "dbo.Users");
            DropIndex("dbo.Brands", new[] { "UserId" });
            DropColumn("dbo.Products", "IsActive");
            DropColumn("dbo.Brands", "UserId");
        }
    }
}
