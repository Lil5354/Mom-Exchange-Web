// Controllers/ClearanceController.cs
using MomExchange.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MomExchange.Controllers
{
    public class ClearanceController : Controller
    {
        // GET: Clearance
        public ActionResult Index()
        {
            // Lấy tất cả sản phẩm giả lập
            var allProducts = GetSampleProducts();

            // Nhóm các sản phẩm theo từng danh mục
            var viewModel = allProducts
                .GroupBy(p => p.Category)
                .Select(g => new CategoryViewModel
                {
                    CategoryName = g.Key, // Tên danh mục (ví dụ: "Quần áo")
                    Products = g.ToList() // Danh sách các sản phẩm trong danh mục đó
                }).ToList();

            return View(viewModel);
        }

        // --- Danh sách sản phẩm giả lập (bạn có thể lấy từ ProductController hoặc CSDL sau này) ---
        private List<Product> GetSampleProducts()
        {
            // (Đây là danh sách sản phẩm giả lập, bạn có thể thêm nhiều sản phẩm hơn cho mỗi danh mục)
            return new List<Product>
            {
                // Danh mục: Quần áo
                new Product { Id = 3, Name = "Set body sơ sinh", Category = "Quần áo", Price = "Trao đổi", ImageUrls = new List<string> { "https://i.pinimg.com/1200x/78/29/91/7829917da73ef1917f7b50adc409a37a.jpg" }, SellerName="Mẹ Sóc" },
                new Product { Id = 5, Name = "Váy công chúa Elsa", Category = "Quần áo", Price = "250.000đ", ImageUrls = new List<string> { "https://i.pinimg.com/564x/e7/07/95/e70795b451c5f6e52271c77501a37c95.jpg" }, SellerName="Mẹ Kem" },
                new Product { Id = 6, Name = "Bộ quần áo Carter", Category = "Quần áo", Price = "150.000đ", ImageUrls = new List<string> { "https://i.pinimg.com/564x/0a/6e/f3/0a6ef32d363574c37d6e6ab334b3e83b.jpg" }, SellerName="Mẹ Gấu" },

                // Danh mục: Đồ chơi, Sách
                new Product { Id = 7, Name = "Bộ đồ chơi gỗ", Category = "Đồ chơi, Sách", Price = "300.000đ", ImageUrls = new List<string> { "https://i.pinimg.com/564x/0f/52/65/0f52654139a13a82e987178d8a77a941.jpg" }, SellerName="Mẹ Bắp" },
                new Product { Id = 8, Name = "Sách ehon cho bé", Category = "Đồ chơi, Sách", Price = "Trao đổi", ImageUrls = new List<string> { "https://i.pinimg.com/564x/53/78/3f/53783f06aa0a022420367a7b8813a34a.jpg" }, SellerName="Mẹ Tít" },

                // Danh mục: Xe đẩy, Nôi cũi
                new Product { Id = 1, Name = "Xe đẩy em bé Aprica", Category = "Xe đẩy, Nôi cũi", Price = "1.200.000₫", ImageUrls = new List<string> { "https://www.kidsplaza.vn/media/catalog/product/a/p/aprica-kroon-hong.jpg" }, SellerName="Mẹ Bắp" },
                new Product { Id = 4, Name = "Ghế ăn dặm cho bé", Category = "Xe đẩy, Nôi cũi", Price = "400.000₫", ImageUrls = new List<string> { "https://songlongplastic.net/wp-content/uploads/2020/01/gh%E1%BA%BF-%C4%83n-%C4%91a-n%C4%83ng-tr%E1%BA%BB-em-song-long-2575-2.jpg" }, SellerName="Mẹ An" },
            };
        }
    }
}