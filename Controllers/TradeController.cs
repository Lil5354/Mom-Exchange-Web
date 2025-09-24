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
    }
}