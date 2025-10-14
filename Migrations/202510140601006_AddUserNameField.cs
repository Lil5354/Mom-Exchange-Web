    namespace B_M.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserNameField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "UserName", c => c.String(maxLength: 50));
            CreateIndex("dbo.Users", "UserName", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Users", new[] { "UserName" });
            DropColumn("dbo.Users", "UserName");
        }
    }
}
