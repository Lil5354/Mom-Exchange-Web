namespace B_M.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using B_M.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "B_M.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            // Ví dụ: Tạo user admin mẫu
            /*
            context.Users.AddOrUpdate(
                u => u.Email,
                new User
                {
                    Email = "admin@momexchange.com",
                    PasswordHash = "hashed_password_here",
                    Role = 1, // Admin
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                    UserDetails = new UserDetails
                    {
                        FullName = "Administrator",
                        ReputationScore = 100
                    }
                }
            );
            */
        }
    }
}

