// Controllers/CommunityController.cs
using MomExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MomExchange.Controllers
{
    public class BlogController : Controller
    {
        // GET: Community
        public ActionResult Index()
        {
            var posts = GetSamplePosts();
            return View(posts);
        }

        // GET: Community/Details/1
        public ActionResult Details(int id)
        {
            var post = GetSamplePosts().FirstOrDefault(p => p.Id == id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        private List<CommunityPost> GetSamplePosts()
        {
            return new List<CommunityPost>
            {
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
            };
        }
    }
}