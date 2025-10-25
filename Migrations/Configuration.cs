namespace B_M.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using B_M.Data;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "B_M.Data.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            SeedData.Seed(context);
        }
    }
}

