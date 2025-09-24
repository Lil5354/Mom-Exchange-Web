// Controllers/TradeController.cs
using MomExchange.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MomExchange.Controllers
{
    public class TradeController : Controller
    {
        // GET: Trade
        public ActionResult Index()
        {
            var tradePosts = GetSampleTradePosts();
            return View(tradePosts);
        }

        private List<TradePost> GetSampleTradePosts()
        {
            return new List<TradePost>
            {
                new TradePost {
                    Id = 1,
                    ItemToOffer = new Product {
                        Id = 7,
                        Name = "Bộ đồ chơi gỗ an toàn",
                        ImageUrls = new List<string> { "https://i.pinimg.com/1200x/59/18/ca/5918ca7f20164b733a70cbb7c8a281b9.jpg" }
                    },
                    DesiredItemsDescription = "Váy bé gái size 1-2 tuổi; Sách Ehon cho bé; Bỉm Bobby size L",
                    SellerName = "Mẹ Tít",
                    SellerAvatarUrl = "/images/avatar1.jpg",
                    Location = "Quận 1, TP.HCM"
                },
                new TradePost {
                    Id = 2,
                    ItemToOffer = new Product {
                        Id = 2,
                        Name = "Máy hút sữa Medela",
                        ImageUrls = new List<string> { "https://www.moby.com.vn/data/bt6/may-hut-sua-dien-don-medela-swing-1594126735.png" }
                    },
                    DesiredItemsDescription = "Ghế ăn dặm cho bé; Xe tập đi; Vitamin D3K2",
                    SellerName = "Mẹ Gấu",
                    SellerAvatarUrl = "/images/avatar2.jpg",
                    Location = "Quận Cầu Giấy, Hà Nội"
                }
            };
        }
        [HttpGet]
        public JsonResult GetUserProductsForTrade()
        {
            // Giả lập danh sách sản phẩm của người dùng đang đăng nhập
            var userProducts = new List<Product>
        {
            new Product { Id = 20, Name = "Bình sữa Comotomo 250ml", ImageUrls = new List<string> {"https://i.pinimg.com/564x/e7/87/04/e78704a259c43d502758204b34b42d76.jpg"} },
            new Product { Id = 21, Name = "Ghế rung cho bé", ImageUrls = new List<string> {"https://i.pinimg.com/564x/bd/6d/46/bd6d4677e5627a7c1340129a393c3c13.jpg"} },
            new Product { Id = 22, Name = "Địu em bé Ergobaby", ImageUrls = new List<string> {"https://i.pinimg.com/564x/a4/0a/85/a40a85011680153f345862d22a5786c8.jpg"} }
        };

            // Trả về dữ liệu dưới dạng JSON
            return Json(userProducts, JsonRequestBehavior.AllowGet);
        }
    }
}