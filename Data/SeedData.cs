using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using B_M.Models.Entities;
using B_M.Helpers; // Required for PasswordHelper

namespace B_M.Data
{
    public class SeedData
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Seed Users and UserDetails
            SeedUsers(context);
            
            // Seed Brands with UserId relationships
            context.Brands.AddOrUpdate(
                b => b.Name,
                new Brand { Id = 1, Name = "Bobby", Description = "Tã bỉm Nhật Bản số 1 Việt Nam", LogoUrl = "/images/logo-bobby.png", UserId = 4 },
                new Brand { Id = 2, Name = "Huggies", Description = "Thoải mái cho bé, an tâm cho mẹ", LogoUrl = "/images/logo-huggies.png", UserId = 5 },
                new Brand { Id = 3, Name = "Moony", Description = "Tã dán siêu mềm, siêu thấm hút", LogoUrl = "/images/logo-moony.png", UserId = 6 },
                new Brand { Id = 4, Name = "Aptamil", Description = "Dinh dưỡng chuyên sâu từ Anh Quốc", LogoUrl = "/images/logo-aptamil.png", UserId = 7 },
                new Brand { Id = 5, Name = "Pigeon", Description = "Sản phẩm chăm sóc mẹ và bé toàn diện", LogoUrl = "/images/logo-pigeon.png", UserId = 8 },
                new Brand { Id = 6, Name = "Comotomo", Description = "Bình sữa silicon cao cấp cho bé", LogoUrl = "/images/logo-comotomo.png", UserId = 9 },
                new Brand { Id = 7, Name = "BioGaia", Description = "Men vi sinh cho hệ tiêu hóa khỏe mạnh", LogoUrl = "/images/logo-biogaia.png", UserId = 10 },
                new Brand { Id = 8, Name = "Chicco", Description = "Thương hiệu mẹ và bé từ Ý", LogoUrl = "/images/logo-chicco.png", UserId = 11 },
                new Brand { Id = 9, Name = "MomExchange Brand", Description = "Thương hiệu chính thức của MomExchange", LogoUrl = "/images/logo-momexchange.png", UserId = 2 }
            );
            
            // Save Brands first
            context.SaveChanges();

            // Seed Products
            var bobbyBrand = context.Brands.FirstOrDefault(b => b.Name == "Bobby");
            var huggiesBrand = context.Brands.FirstOrDefault(b => b.Name == "Huggies");
            var moonyBrand = context.Brands.FirstOrDefault(b => b.Name == "Moony");
            var aptamilBrand = context.Brands.FirstOrDefault(b => b.Name == "Aptamil");
            var pigeonBrand = context.Brands.FirstOrDefault(b => b.Name == "Pigeon");
            var comotomoBrand = context.Brands.FirstOrDefault(b => b.Name == "Comotomo");
            var biogaiaBrand = context.Brands.FirstOrDefault(b => b.Name == "BioGaia");
            var chiccoBrand = context.Brands.FirstOrDefault(b => b.Name == "Chicco");
            var momexchangeBrand = context.Brands.FirstOrDefault(b => b.Name == "MomExchange Brand");
            
            if (bobbyBrand != null && huggiesBrand != null && moonyBrand != null && 
                aptamilBrand != null && pigeonBrand != null && comotomoBrand != null && 
                biogaiaBrand != null && chiccoBrand != null && momexchangeBrand != null)
            {
                context.Products.AddOrUpdate(
                    p => p.Name,
                    new Product {
                        Id = 1,
                        Name = "Xe đẩy em bé Aprica",
                        Category = "Xe đẩy, Nôi cũi",
                        Price = "1.200.000₫",
                        ShortDescription = "Xe đẩy Aprica nội địa Nhật, còn mới 95%, đầy đủ phụ kiện. Gấp gọn dễ dàng, phù hợp cho bé từ sơ sinh đến 3 tuổi.",
                        DetailedDescription = "Xe nhà mình dùng kỹ nên còn mới đến 95%, không có lỗi hỏng, trầy xước không đáng kể. Vải nệm đã được giặt sạch sẽ, thơm tho, bé có thể dùng ngay.",
                        Condition = "Đã sử dụng (còn mới 95%)",
                        BrandId = bobbyBrand.Id,
                        Location = "Quận Bình Tân, TP. Hồ Chí Minh",
                        SellerName = "Mẹ Bắp",
                        SellerAvatarUrl = "https://via.placeholder.com/50/fdeee9/e15b7f?Text=B",
                        SellerRating = 4.8,
                        SellerReviewCount = 25,
                        ImageUrls = new List<string> {
                            "https://www.kidsplaza.vn/media/catalog/product/a/p/aprica-kroon-hong.jpg",
                            "https://i.pinimg.com/564x/0f/52/24/0f522434255152a450125aa5e6f54c2e.jpg",
                            "https://i.pinimg.com/564x/1f/26/1c/1f261cb95a3299727ed248e3e414c719.jpg"
                        }
                    },
                    new Product {
                        Id = 2,
                        Name = "Máy hút sữa Medela",
                        Category = "Máy hút sữa & Dụng cụ",
                        Price = "850.000₫",
                        ShortDescription = "Máy hút sữa Medela Swing, lực hút mạnh, êm ái, đầy đủ phụ kiện tiệt trùng. Tặng kèm túi trữ sữa.",
                        DetailedDescription = "Máy còn hoạt động rất tốt, pin bền. Mình đã vệ sinh và tiệt trùng tất cả các bộ phận. Mua về là dùng được ngay.",
                        Condition = "Đã sử dụng (còn mới 90%)",
                        BrandId = huggiesBrand.Id,
                        Location = "Quận Cầu Giấy, Hà Nội",
                        SellerName = "Mẹ Gấu",
                        SellerAvatarUrl = "https://via.placeholder.com/50/e0f7fa/009688?Text=G",
                        SellerRating = 4.9,
                        SellerReviewCount = 18,
                        ImageUrls = new List<string> {
                            "https://www.moby.com.vn/data/bt6/may-hut-sua-dien-don-medela-swing-1594126735.png",
                            "https://i.pinimg.com/564x/a3/9a/b9/a39ab99e1c3182f2c2534125b1e6a1d4.jpg",
                            "https://i.pinimg.com/564x/7e/76/87/7e76878b665324545d7f1d4187f4c092.jpg"
                        }
                    },
                    new Product {
                        Id = 3,
                        Name = "Set body sơ sinh",
                        Category = "Quần áo",
                        Price = "Trao đổi",
                        ShortDescription = "5 bộ body Nous cộc tay cho bé trai 3-6 tháng. Chất vải petit siêu mềm mát, thấm hút mồ hôi tốt.",
                        DetailedDescription = "Đồ bé mình mặc hơi chật, mới mặc 1-2 lần nên còn rất mới, không bị xù lông hay dão. Mình muốn đổi lấy đồ cho bé gái hoặc đồ chơi gỗ.",
                        Condition = "Đã sử dụng (còn mới 98%)",
                        BrandId = moonyBrand.Id,
                        Location = "Quận Hải Châu, Đà Nẵng",
                        SellerName = "Mẹ Sóc",
                        SellerAvatarUrl = "https://via.placeholder.com/50/fff9c4/fbc02d?Text=S",
                        SellerRating = 5.0,
                        SellerReviewCount = 31,
                        ImageUrls = new List<string> {
                            "https://i.pinimg.com/1200x/78/29/91/7829917da73ef1917f7b50adc409a37a.jpg",
                            "https://i.pinimg.com/564x/4b/36/4f/4b364f514d021c72b225330e7f722026.jpg",
                            "https://i.pinimg.com/564x/2c/80/08/2c800889f07297e59443e215458097d6.jpg"
                        }
                    },
                    new Product { 
                        Id = 10, 
                        Name = "Tã dán Bobby size M", 
                        BrandId = bobbyBrand.Id,
                        Price = "185.000₫", 
                        ImageUrls = new List<string> { "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRlw-3TTe4si5ZpUNLulfDWkUK6wy9NkXnfqA&s" }, 
                        SellerName = "Nhà phân phối chính hãng" 
                    },
                    new Product { 
                        Id = 11, 
                        Name = "Tã quần Bobby size L", 
                        BrandId = bobbyBrand.Id,
                        Price = "210.000₫", 
                        ImageUrls = new List<string> { "https://cdn-v2.kidsplaza.vn/media/catalog/product/b/i/bim-ta-quan-bobby-fresh-size-l-68-mieng-cho-be-9-13kg-a.jpg" }, 
                        SellerName = "Nhà phân phối chính hãng" 
                    },
                    new Product { 
                        Id = 12, 
                        Name = "Tã dán Huggies Platinum", 
                        BrandId = huggiesBrand.Id,
                        Price = "350.000₫", 
                        ImageUrls = new List<string> { "https://www.huggies.com.vn/-/media/Project/HuggiesVN/Images/Product/ta-dan-huggies-platinum-naturemade/ta-dan-huggies-platinum-naturemade-size-xl-pdp-image.jpg" }, 
                        SellerName = "Nhà phân phối chính hãng" 
                    },
                    new Product { 
                        Id = 13, 
                        Name = "Sữa Aptamil Profutura 1", 
                        BrandId = aptamilBrand.Id,
                        Price = "650.000₫", 
                        ImageUrls = new List<string> { "https://concung.com/2024/05/64564-110429-large_mobile/aptamil-profutura-cesarbiotik-1-800g-0-12-thang.png" }, 
                        SellerName = "Nhà phân phối chính hãng" 
                    },
                    new Product { 
                        Id = 14, 
                        Name = "Bộ sản phẩm MomExchange Premium", 
                        BrandId = momexchangeBrand.Id,
                        Price = "1.200.000₫", 
                        ImageUrls = new List<string> { "https://via.placeholder.com/300x300/d63384/ffffff?Text=MomExchange" }, 
                        SellerName = "MomExchange Official",
                        IsActive = true
                    },
                    new Product { 
                        Id = 15, 
                        Name = "Gói dịch vụ tư vấn MomExchange", 
                        BrandId = momexchangeBrand.Id,
                        Price = "500.000₫", 
                        ImageUrls = new List<string> { "https://via.placeholder.com/300x300/e91e63/ffffff?Text=Consulting" }, 
                        SellerName = "MomExchange Official",
                        IsActive = true
                    }
                );
            }
            
            // Save Products
            context.SaveChanges();
            
            // Seed Product Images
            SeedProductImages(context);

            // Seed Community Posts
            context.CommunityPosts.AddOrUpdate(
                cp => cp.Title,
                new CommunityPost
                {
                    Id = 1,
                    Title = "Kinh nghiệm chọn xe đẩy cho bé lần đầu làm mẹ",
                    AuthorName = "Mẹ Sóc",
                    AuthorAvatarUrl = "/images/avatar1.jpg",
                    PostDate = DateTime.Now.AddDays(-2),
                    ImageUrl = "https://xedayembenhatban.com/wp-content/uploads/2019/03/xe-day-second-hand-tp-hcm-4.jpg",
                    Excerpt = "Chọn xe đẩy cho bé yêu lần đầu quả thực không dễ dàng. Giữa vô vàn lựa chọn, đâu mới là chiếc xe đẩy phù hợp nhất? Hãy cùng mình tìm hiểu nhé...",
                    Content = "<p>Chọn xe đẩy cho bé yêu lần đầu quả thực không dễ dàng. Giữa vô vàn lựa chọn, đâu mới là chiếc xe đẩy phù hợp nhất? Hãy cùng mình tìm hiểu nhé...</p><p>Yếu tố đầu tiên mình quan tâm là độ an toàn. Khung xe phải chắc chắn, có đai an toàn 5 điểm và phanh xe hoạt động tốt. Tiếp theo là sự tiện lợi, xe nên có thể gấp gọn dễ dàng để mang theo khi đi du lịch hoặc cất vào cốp xe...</p>",
                    Tags = new List<string> { "Kinh nghiệm", "Xe đẩy" }
                },
                new CommunityPost
                {
                    Id = 2,
                    Title = "Review máy hâm sữa: Đâu là chân ái cho mẹ bỉm?",
                    AuthorName = "Mẹ Bắp",
                    AuthorAvatarUrl = "/images/avatar2.jpg",
                    PostDate = DateTime.Now.AddDays(-5),
                    ImageUrl = "https://vinasave.com/vnt_upload/news/05_2018/mua-ban-thanh-ly-may-ham-sua-cu-cho-be-2.jpg",
                    Excerpt = "Máy hâm sữa là một trợ thủ đắc lực không thể thiếu. Nhưng nên chọn loại nào? Cùng mình điểm qua vài dòng máy phổ biến và tìm ra chân ái nhé!",
                    Content = "<p>Máy hâm sữa là một trợ thủ đắc lực không thể thiếu. Nhưng nên chọn loại nào? Cùng mình điểm qua vài dòng máy phổ biến và tìm ra chân ái nhé!</p><p>Mình đã thử qua 3 loại: Fatzbaby, Philips Avent và Beurer. Mỗi loại đều có ưu nhược điểm riêng. Fatzbaby có giá thành rẻ, nhiều chức năng nhưng độ bền không cao. Philips Avent thì hâm sữa rất nhanh và đều nhưng giá lại khá \"chát\"...</p>",
                    Tags = new List<string> { "Review", "Dụng cụ" }
                }
            );
            
            // Save Community Posts
            context.SaveChanges();

            // Seed Social Posts
            context.SocialPosts.AddOrUpdate(
                sp => sp.Content,
                new SocialPost
                {
                    Id = 1,
                    AuthorName = "Mẹ Bối Bối",
                    AuthorAvatarUrl = "/images/avatar2.jpg",
                    PostTime = "2 giờ trước",
                    Content = "Các mẹ ơi, có ai có kinh nghiệm dùng địu cho bé dưới 6 tháng không ạ? Mình đang phân vân giữa Ergobaby và Aprica quá. Cho mình xin ít review với ạ. Cảm ơn các mẹ nhiều! ❤️",
                    ImageUrl = "https://i.pinimg.com/564x/a4/0a/85/a40a85011680153f345862d22a5786c8.jpg",
                    LikeCount = 15,
                    CommentCount = 8
                },
                new SocialPost
                {
                    Id = 2,
                    AuthorName = "Mẹ Sóc",
                    AuthorAvatarUrl = "https://via.placeholder.com/50/fff9c4/fbc02d?Text=S",
                    PostTime = "Hôm qua",
                    Content = "Chào cả nhà, em vừa làm món ruốc cá hồi này cho bé ăn dặm, trộm vía con thích lắm ạ. Mẹ nào cần công thức không em chia sẻ nhé!",
                    ImageUrl = "https://i.pinimg.com/564x/1a/ac/8a/1aac8ae12800b73c47a09282216a6176.jpg",
                    LikeCount = 42,
                    CommentCount = 19
                }
            );
            
            // Save Social Posts
            context.SaveChanges();

            // Seed Trade Posts
            var product7 = context.Products.FirstOrDefault(p => p.Name == "Bộ đồ chơi gỗ an toàn");
            var product2 = context.Products.FirstOrDefault(p => p.Name == "Máy hút sữa Medela");
            
            if (product7 != null && product2 != null)
            {
                context.TradePosts.AddOrUpdate(
                    tp => tp.SellerName,
                    new TradePost {
                        Id = 1,
                        ItemToOffer = product7,
                        DesiredItemsDescription = "Váy bé gái size 1-2 tuổi; Sách Ehon cho bé; Bỉm Bobby size L",
                        SellerName = "Mẹ Tít",
                        SellerAvatarUrl = "/images/avatar1.jpg",
                        Location = "Quận 1, TP.HCM"
                    },
                    new TradePost {
                        Id = 2,
                        ItemToOffer = product2,
                        DesiredItemsDescription = "Ghế ăn dặm cho bé; Xe tập đi; Vitamin D3K2",
                        SellerName = "Mẹ Gấu",
                        SellerAvatarUrl = "/images/avatar2.jpg",
                        Location = "Quận Cầu Giấy, Hà Nội"
                    }
                );
            }
            
            // Save Trade Posts
            context.SaveChanges();

            // Seed Milk Donation Posts
            context.MilkDonationPosts.AddOrUpdate(
                mdp => mdp.DonorName,
                new MilkDonationPost {
                    Id = 1,
                    DonorName = "Mẹ An Nhiên",
                    Location = "Quận 1, TP.HCM",
                    DateOfExpression = new DateTime(2025, 9, 22),
                    DietInfo = "Ăn uống đa dạng, đủ chất, không sử dụng chất kích thích. Uống vitamin tổng hợp.",
                    StorageInfo = "Sữa được hút bằng máy Medela, trữ trong túi ZipLock chuyên dụng và cấp đông ngay trong tủ đông -18°C.",
                    Note = "Mình có nhiều sữa nên muốn chia sẻ cho các bé có nhu cầu. Chỉ nhận trao đổi tại nhà.",
                    DonorAvatarUrl = "https://i.pinimg.com/1200x/7e/43/35/7e4335dbd0265d9b027ee31ca69e2702.jpg",
                    IsHealthVerified = true
                },
                new MilkDonationPost {
                    Id = 2,
                    DonorName = "Mẹ Bối Bối",
                    Location = "Quận Ba Đình, Hà Nội",
                    DateOfExpression = new DateTime(2025, 9, 20),
                    DietInfo = "Chế độ ăn uống bình thường, lành mạnh.",
                    StorageInfo = "Trữ đông trong tủ lạnh gia đình.",
                    Note = "Sữa cho bé trai, mong muốn tặng cho các mẹ có hoàn cảnh khó khăn.",
                    DonorAvatarUrl = "https://i.pinimg.com/1200x/8f/d7/d6/8fd7d605b7a9ba192913746bf692865b.jpg",
                    IsHealthVerified = false
                }
            );
            
            // Save Milk Donation Posts
            context.SaveChanges();
        }

        private static void SeedUsers(ApplicationDbContext context)
        {
            // Hash password: Password@123 using system's PasswordHelper
            string hashedPassword = PasswordHelper.HashPassword("Password@123");

            // Seed Users
            context.Users.AddOrUpdate(
                u => u.Email,
                new User
                {
                    UserID = 1,
                    UserName = "admin",
                    Email = "admin@momexchange.com",
                    PhoneNumber = "0123456789",
                    PasswordHash = hashedPassword,
                    Role = 1, // Admin
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-30)
                },
                new User
                {
                    UserID = 2,
                    UserName = "brand_manager",
                    Email = "brand@momexchange.com",
                    PhoneNumber = "0987654321",
                    PasswordHash = hashedPassword,
                    Role = 3, // Brand
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-20)
                },
                new User
                {
                    UserID = 3,
                    UserName = "mom_user",
                    Email = "mom@momexchange.com",
                    PhoneNumber = "0369258147",
                    PasswordHash = hashedPassword,
                    Role = 2, // Mom
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-10)
                },
                // Brand user accounts
                new User
                {
                    UserID = 4,
                    UserName = "bobby_brand",
                    Email = "bobby@momexchange.com",
                    PhoneNumber = "0901234567",
                    PasswordHash = hashedPassword,
                    Role = 3, // Brand
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-25)
                },
                new User
                {
                    UserID = 5,
                    UserName = "huggies_brand",
                    Email = "huggies@momexchange.com",
                    PhoneNumber = "0901234568",
                    PasswordHash = hashedPassword,
                    Role = 3, // Brand
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-24)
                },
                new User
                {
                    UserID = 6,
                    UserName = "moony_brand",
                    Email = "moony@momexchange.com",
                    PhoneNumber = "0901234569",
                    PasswordHash = hashedPassword,
                    Role = 3, // Brand
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-23)
                },
                new User
                {
                    UserID = 7,
                    UserName = "aptamil_brand",
                    Email = "aptamil@momexchange.com",
                    PhoneNumber = "0901234570",
                    PasswordHash = hashedPassword,
                    Role = 3, // Brand
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-22)
                },
                new User
                {
                    UserID = 8,
                    UserName = "pigeon_brand",
                    Email = "pigeon@momexchange.com",
                    PhoneNumber = "0901234571",
                    PasswordHash = hashedPassword,
                    Role = 3, // Brand
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-21)
                },
                new User
                {
                    UserID = 9,
                    UserName = "comotomo_brand",
                    Email = "comotomo@momexchange.com",
                    PhoneNumber = "0901234572",
                    PasswordHash = hashedPassword,
                    Role = 3, // Brand
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-20)
                },
                new User
                {
                    UserID = 10,
                    UserName = "biogaia_brand",
                    Email = "biogaia@momexchange.com",
                    PhoneNumber = "0901234573",
                    PasswordHash = hashedPassword,
                    Role = 3, // Brand
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-19)
                },
                new User
                {
                    UserID = 11,
                    UserName = "chicco_brand",
                    Email = "chicco@momexchange.com",
                    PhoneNumber = "0901234574",
                    PasswordHash = hashedPassword,
                    Role = 3, // Brand
                    IsActive = true,
                    CreatedAt = DateTime.Now.AddDays(-18)
                }
            );

            // Save Users first
            context.SaveChanges();

            // Seed UserDetails
            context.UserDetails.AddOrUpdate(
                ud => ud.UserID,
                new UserDetails
                {
                    UserID = 1,
                    FullName = "Quản trị viên hệ thống",
                    ProfilePictureURL = "https://via.placeholder.com/100/ff6b6b/ffffff?Text=A",
                    Address = "TP. Hồ Chí Minh",
                    ReputationScore = 5.0
                },
                new UserDetails
                {
                    UserID = 2,
                    FullName = "MomExchange Brand Manager",
                    ProfilePictureURL = "/images/logo-momexchange.png",
                    Address = "TP. Hồ Chí Minh",
                    ReputationScore = 5.0
                },
                new UserDetails
                {
                    UserID = 3,
                    FullName = "Mẹ Bắp",
                    ProfilePictureURL = "https://via.placeholder.com/100/45b7d1/ffffff?Text=M",
                    Address = "Quận Bình Tân, TP. Hồ Chí Minh",
                    ReputationScore = 4.9
                },
                // Brand user details
                new UserDetails
                {
                    UserID = 4,
                    FullName = "Bobby Vietnam",
                    ProfilePictureURL = "/images/logo-bobby.png",
                    Address = "TP. Hồ Chí Minh",
                    ReputationScore = 4.9
                },
                new UserDetails
                {
                    UserID = 5,
                    FullName = "Huggies Vietnam",
                    ProfilePictureURL = "/images/logo-huggies.png",
                    Address = "Hà Nội",
                    ReputationScore = 4.8
                },
                new UserDetails
                {
                    UserID = 6,
                    FullName = "Moony Vietnam",
                    ProfilePictureURL = "/images/logo-moony.png",
                    Address = "TP. Hồ Chí Minh",
                    ReputationScore = 4.7
                },
                new UserDetails
                {
                    UserID = 7,
                    FullName = "Aptamil Vietnam",
                    ProfilePictureURL = "/images/logo-aptamil.png",
                    Address = "Hà Nội",
                    ReputationScore = 4.9
                },
                new UserDetails
                {
                    UserID = 8,
                    FullName = "Pigeon Vietnam",
                    ProfilePictureURL = "/images/logo-pigeon.png",
                    Address = "TP. Hồ Chí Minh",
                    ReputationScore = 4.6
                },
                new UserDetails
                {
                    UserID = 9,
                    FullName = "Comotomo Vietnam",
                    ProfilePictureURL = "/images/logo-comotomo.png",
                    Address = "Hà Nội",
                    ReputationScore = 4.8
                },
                new UserDetails
                {
                    UserID = 10,
                    FullName = "BioGaia Vietnam",
                    ProfilePictureURL = "/images/logo-biogaia.png",
                    Address = "TP. Hồ Chí Minh",
                    ReputationScore = 4.7
                },
                new UserDetails
                {
                    UserID = 11,
                    FullName = "Chicco Vietnam",
                    ProfilePictureURL = "/images/logo-chicco.png",
                    Address = "Hà Nội",
                    ReputationScore = 4.8
                }
            );

            // Save UserDetails
            context.SaveChanges();
        }

        private static void SeedProductImages(ApplicationDbContext context)
        {
            // Get products by name to avoid hardcoded IDs
            var product1 = context.Products.FirstOrDefault(p => p.Name == "Xe đẩy em bé Aprica");
            var product2 = context.Products.FirstOrDefault(p => p.Name == "Máy hút sữa Medela");
            var product3 = context.Products.FirstOrDefault(p => p.Name == "Set body sơ sinh");
            var product10 = context.Products.FirstOrDefault(p => p.Name == "Tã dán Bobby size M");
            var product11 = context.Products.FirstOrDefault(p => p.Name == "Tã quần Bobby size L");
            var product12 = context.Products.FirstOrDefault(p => p.Name == "Tã dán Huggies Platinum");
            var product13 = context.Products.FirstOrDefault(p => p.Name == "Sữa Aptamil Profutura 1");

            if (product1 != null)
            {
                context.ProductImages.AddOrUpdate(
                    pi => new { pi.ProductId, pi.ImageUrl },
                    new ProductImage { ProductId = product1.Id, ImageUrl = "https://www.kidsplaza.vn/media/catalog/product/a/p/aprica-kroon-hong.jpg", SortOrder = 0 },
                    new ProductImage { ProductId = product1.Id, ImageUrl = "https://i.pinimg.com/564x/0f/52/24/0f522434255152a450125aa5e6f54c2e.jpg", SortOrder = 1 },
                    new ProductImage { ProductId = product1.Id, ImageUrl = "https://i.pinimg.com/564x/1f/26/1c/1f261cb95a3299727ed248e3e414c719.jpg", SortOrder = 2 }
                );
            }

            if (product2 != null)
            {
                context.ProductImages.AddOrUpdate(
                    pi => new { pi.ProductId, pi.ImageUrl },
                    new ProductImage { ProductId = product2.Id, ImageUrl = "https://www.moby.com.vn/data/bt6/may-hut-sua-dien-don-medela-swing-1594126735.png", SortOrder = 0 },
                    new ProductImage { ProductId = product2.Id, ImageUrl = "https://i.pinimg.com/564x/a3/9a/b9/a39ab99e1c3182f2c2534125b1e6a1d4.jpg", SortOrder = 1 },
                    new ProductImage { ProductId = product2.Id, ImageUrl = "https://i.pinimg.com/564x/7e/76/87/7e76878b665324545d7f1d4187f4c092.jpg", SortOrder = 2 }
                );
            }

            if (product3 != null)
            {
                context.ProductImages.AddOrUpdate(
                    pi => new { pi.ProductId, pi.ImageUrl },
                    new ProductImage { ProductId = product3.Id, ImageUrl = "https://i.pinimg.com/1200x/78/29/91/7829917da73ef1917f7b50adc409a37a.jpg", SortOrder = 0 },
                    new ProductImage { ProductId = product3.Id, ImageUrl = "https://i.pinimg.com/564x/4b/36/4f/4b364f514d021c72b225330e7f722026.jpg", SortOrder = 1 },
                    new ProductImage { ProductId = product3.Id, ImageUrl = "https://i.pinimg.com/564x/2c/80/08/2c800889f07297e59443e215458097d6.jpg", SortOrder = 2 }
                );
            }

            if (product10 != null)
            {
                context.ProductImages.AddOrUpdate(
                    pi => new { pi.ProductId, pi.ImageUrl },
                    new ProductImage { ProductId = product10.Id, ImageUrl = "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRlw-3TTe4si5ZpUNLulfDWkUK6wy9NkXnfqA&s", SortOrder = 0 }
                );
            }

            if (product11 != null)
            {
                context.ProductImages.AddOrUpdate(
                    pi => new { pi.ProductId, pi.ImageUrl },
                    new ProductImage { ProductId = product11.Id, ImageUrl = "https://cdn-v2.kidsplaza.vn/media/catalog/product/b/i/bim-ta-quan-bobby-fresh-size-l-68-mieng-cho-be-9-13kg-a.jpg", SortOrder = 0 }
                );
            }

            if (product12 != null)
            {
                context.ProductImages.AddOrUpdate(
                    pi => new { pi.ProductId, pi.ImageUrl },
                    new ProductImage { ProductId = product12.Id, ImageUrl = "https://www.huggies.com.vn/-/media/Project/HuggiesVN/Images/Product/ta-dan-huggies-platinum-naturemade/ta-dan-huggies-platinum-naturemade-size-xl-pdp-image.jpg", SortOrder = 0 }
                );
            }

            if (product13 != null)
            {
                context.ProductImages.AddOrUpdate(
                    pi => new { pi.ProductId, pi.ImageUrl },
                    new ProductImage { ProductId = product13.Id, ImageUrl = "https://concung.com/2024/05/64564-110429-large_mobile/aptamil-profutura-cesarbiotik-1-800g-0-12-thang.png", SortOrder = 0 }
                );
            }

            // Save Product Images
            context.SaveChanges();
        }
    }
}