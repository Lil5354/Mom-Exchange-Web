// Controllers/MilkDonationController.cs
using B_M.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace B_M.Controllers
{
    public class MilkDonationController : Controller
    {
        // GET: MilkDonation
        public ActionResult Index()
        {
            var posts = GetSamplePosts();
            return View(posts);
        }

        // GET: MilkDonation/Details/1
        public ActionResult Details(int id)
        {
            var post = GetSamplePosts().FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        private List<MilkDonationPost> GetSamplePosts()
        {
            return new List<MilkDonationPost>
            {
                new MilkDonationPost {
                    Id = 1,
                    DonorName = "Mẹ An Nhiên",
                    Location = "Quận 1, TP.HCM",
                    DateOfExpression = new DateTime(2025, 9, 22),
                    DietInfo = "Ăn uống đa dạng, đủ chất, không sử dụng chất kích thích. Uống vitamin tổng hợp.",
                    StorageInfo = "Sữa được hút bằng máy Medela, trữ trong túi ZipLock chuyên dụng và cấp đông ngay trong tủ đông -18°C.",
                    Note = "Mình có nhiều sữa nên muốn chia sẻ cho các bé có nhu cầu. Chỉ nhận trao đổi tại nhà.",
                    DonorAvatarUrl = "https://i.pinimg.com/1200x/7e/43/35/7e4335dbd0265d9b027ee31ca69e2702.jpg",
                    IsHealthVerified = true // Mẹ này đã được xác thực
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
                    IsHealthVerified = false // Mẹ này chưa xác thực
                }
            };
        }
    }
}