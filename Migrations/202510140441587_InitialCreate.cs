namespace B_M.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
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
                .ForeignKey("dbo.Users", t => t.UserID, cascadeDelete: true)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserID = c.Int(nullable: false, identity: true),
                        Email = c.String(nullable: false, maxLength: 255),
                        PhoneNumber = c.String(maxLength: 20),
                        PasswordHash = c.String(nullable: false),
                        Role = c.Byte(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        CreatedAt = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.UserID)
                .Index(t => t.Email, unique: true)
                .Index(t => t.PhoneNumber, unique: true);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserDetails", "UserID", "dbo.Users");
            DropIndex("dbo.Users", new[] { "PhoneNumber" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.UserDetails", new[] { "UserID" });
            DropTable("dbo.Users");
            DropTable("dbo.UserDetails");
        }
    }
}
