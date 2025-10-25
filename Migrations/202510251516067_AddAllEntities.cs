namespace B_M.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAllEntities : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Brands",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LogoUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Category = c.String(),
                        Price = c.String(),
                        ShortDescription = c.String(),
                        DetailedDescription = c.String(),
                        Condition = c.String(),
                        BrandId = c.Int(nullable: false),
                        Location = c.String(),
                        SellerName = c.String(),
                        SellerAvatarUrl = c.String(),
                        SellerRating = c.Double(nullable: false),
                        SellerReviewCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Brands", t => t.BrandId, cascadeDelete: true)
                .Index(t => t.BrandId);
            
            CreateTable(
                "dbo.CommunityPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        AuthorName = c.String(),
                        AuthorAvatarUrl = c.String(),
                        PostDate = c.DateTime(nullable: false),
                        ImageUrl = c.String(),
                        Excerpt = c.String(),
                        Content = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MilkDonationPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DonorName = c.String(),
                        Location = c.String(),
                        DateOfExpression = c.DateTime(nullable: false),
                        DietInfo = c.String(),
                        StorageInfo = c.String(),
                        Note = c.String(),
                        DonorAvatarUrl = c.String(),
                        IsHealthVerified = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SocialPosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AuthorName = c.String(),
                        AuthorAvatarUrl = c.String(),
                        PostTime = c.String(),
                        Content = c.String(),
                        ImageUrl = c.String(),
                        LikeCount = c.Int(nullable: false),
                        CommentCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TradePosts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DesiredItemsDescription = c.String(),
                        SellerName = c.String(),
                        SellerAvatarUrl = c.String(),
                        Location = c.String(),
                        ItemToOffer_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Products", t => t.ItemToOffer_Id)
                .Index(t => t.ItemToOffer_Id);
            
            CreateTable(
                "dbo.UserDetails",
                c => new
                    {
                        UserID = c.Int(nullable: false),
                        FullName = c.String(nullable: false, maxLength: 100),
                        ProfilePictureURL = c.String(maxLength: 500),
                        Address = c.String(maxLength: 500),
                        ReputationScore = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.UserID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        UserName = c.String(maxLength: 50),
                        Email = c.String(nullable: false, maxLength: 255),
                        PhoneNumber = c.String(maxLength: 20),
                        PasswordHash = c.String(nullable: false),
                        Role = c.Byte(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UserID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDetails", "UserID", "dbo.Users");
            DropForeignKey("dbo.TradePosts", "ItemToOffer_Id", "dbo.Products");
            DropForeignKey("dbo.Products", "BrandId", "dbo.Brands");
            DropIndex("dbo.UserDetails", new[] { "UserID" });
            DropIndex("dbo.TradePosts", new[] { "ItemToOffer_Id" });
            DropIndex("dbo.Products", new[] { "BrandId" });
            DropTable("dbo.Users");
            DropTable("dbo.UserDetails");
            DropTable("dbo.TradePosts");
            DropTable("dbo.SocialPosts");
            DropTable("dbo.MilkDonationPosts");
            DropTable("dbo.CommunityPosts");
            DropTable("dbo.Products");
            DropTable("dbo.Brands");
        }
    }
}
