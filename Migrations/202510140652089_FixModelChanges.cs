namespace B_M.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixModelChanges : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Users", new[] { "UserName" });
            DropIndex("dbo.Users", new[] { "Email" });
            DropIndex("dbo.Users", new[] { "PhoneNumber" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.Users", "PhoneNumber", unique: true);
            CreateIndex("dbo.Users", "Email", unique: true);
            CreateIndex("dbo.Users", "UserName", unique: true);
        }
    }
}
