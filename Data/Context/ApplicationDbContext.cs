// File: Data/Context/ApplicationDbContext.cs
using System.Data.Entity;
using B_M.Models.Entities;

namespace B_M.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base("MomExchangeDB")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<CommunityPost> CommunityPosts { get; set; }
        public DbSet<TradePost> TradePosts { get; set; }
        public DbSet<MilkDonationPost> MilkDonationPosts { get; set; }
        public DbSet<SocialPost> SocialPosts { get; set; }

    }
}

