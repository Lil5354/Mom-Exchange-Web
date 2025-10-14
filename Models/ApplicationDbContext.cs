// File: Models/ApplicationDbContext.cs
using System.Data.Entity;

namespace MomExchange.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("MomExchangeDB")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Cấu hình bảng Users
            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasKey(u => u.UserID);

            modelBuilder.Entity<User>()
                .Property(u => u.UserID)
                .HasColumnName("UserID");

            modelBuilder.Entity<User>()
                .Property(u => u.UserName)
                .HasMaxLength(50);

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(255);

            modelBuilder.Entity<User>()
                .Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.IsActive)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.CreatedAt)
                .IsRequired()
                .HasColumnType("datetime2");

            // Cấu hình bảng UserDetails (quan hệ 1-1 với Users)
            modelBuilder.Entity<UserDetails>()
                .ToTable("UserDetails")
                .HasKey(ud => ud.UserID);

            modelBuilder.Entity<UserDetails>()
                .Property(ud => ud.FullName)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<UserDetails>()
                .Property(ud => ud.ProfilePictureURL)
                .HasMaxLength(500);

            modelBuilder.Entity<UserDetails>()
                .Property(ud => ud.Address)
                .HasMaxLength(500);

            modelBuilder.Entity<UserDetails>()
                .Property(ud => ud.ReputationScore)
                .IsRequired();

            // Thiết lập quan hệ 1-1 giữa User và UserDetails
            modelBuilder.Entity<User>()
                .HasOptional(u => u.UserDetails)
                .WithRequired(ud => ud.User)
                .WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}

