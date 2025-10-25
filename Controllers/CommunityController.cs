// Controllers/CommunityController.cs

using B_M.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B_M.Controllers
{
    public class CommunityController : Controller
    {
        // GET: Community
        public ActionResult Index()
        {
            // Lấy danh sách các bài đăng mẫu
            var posts = GetSampleSocialPosts();

            // Truyền danh sách này đến View
            return View(posts);
        }

        // --- Phương thức tạo dữ liệu mẫu ---
        private List<SocialPost> GetSampleSocialPosts()
        {
            return new List<SocialPost>
            {
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
            };
        }
    }
}