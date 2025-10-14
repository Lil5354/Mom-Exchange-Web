using B_M.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace B_M.Controllers
{
    public class ClearanceController : Controller
    {
        public ActionResult Index()
        {
            var allProducts = GetSampleProducts();

            var viewModel = allProducts
                .GroupBy(p => p.Category)
                .Select(g => new CategoryViewModel
                {
                    CategoryName = g.Key,
                    Products = g.ToList()
                })
                .OrderBy(c => c.CategoryName) // Sắp xếp danh mục theo ABC
                .ToList();

            return View(viewModel);
        }

        private List<Product> GetSampleProducts()
        {
            return new List<Product>
            {
                new Product { Id = 3, Name = "Set body sơ sinh", Category = "Quần áo", Price = "Trao đổi", ImageUrls = new List<string> { "https://i.pinimg.com/1200x/78/29/91/7829917da73ef1917f7b50adc409a37a.jpg" }, SellerName="Mẹ Sóc", Location = "Đà Nẵng" },
                new Product { Id = 5, Name = "Váy công chúa Elsa", Category = "Quần áo", Price = "250.000đ", ImageUrls = new List<string> { "https://i.pinimg.com/736x/9e/ca/f4/9ecaf49b2a1e13822e24689abd4e5b92.jpg" }, SellerName="Mẹ Kem", Location = "TP. HCM" },
                new Product { Id = 6, Name = "Bộ quần áo Carter", Category = "Quần áo", Price = "150.000đ", ImageUrls = new List<string> { "https://i.pinimg.com/1200x/3d/b2/f4/3db2f4adf6a1904fd9a8f8d07b0446bf.jpg" }, SellerName="Mẹ Gấu", Location = "Hà Nội" },
                new Product { Id = 7, Name = "Bộ đồ chơi gỗ", Category = "Đồ chơi, Sách", Price = "300.000đ", ImageUrls = new List<string> { "https://i.pinimg.com/736x/d0/99/f0/d099f07971798530212fbfcb1ac6ac97.jpg" }, SellerName="Mẹ Bắp", Location = "Bình Dương" },
                new Product { Id = 8, Name = "Sách ehon cho bé", Category = "Đồ chơi, Sách", Price = "Trao đổi", ImageUrls = new List<string> { "https://i.pinimg.com/736x/82/d4/0e/82d40ed70af0df480682aac183a23722.jpg" }, SellerName="Mẹ Tít", Location = "Hà Nội" },
                new Product { Id = 1, Name = "Xe đẩy em bé Aprica", Category = "Xe đẩy, Nôi cũi", Price = "1.200.000₫", ImageUrls = new List<string> { "https://www.kidsplaza.vn/media/catalog/product/a/p/aprica-kroon-hong.jpg" }, SellerName="Mẹ Bắp", Location = "TP. HCM" },
                new Product { Id = 4, Name = "Ghế ăn dặm cho bé", Category = "Xe đẩy, Nôi cũi", Price = "400.000₫", ImageUrls = new List<string> { "https://songlongplastic.net/wp-content/uploads/2020/01/gh%E1%BA%BF-%C4%83n-%C4%91a-n%C4%83ng-tr%E1%BA%BB-em-song-long-2575-2.jpg" }, SellerName="Mẹ An", Location = "TP. HCM" },
            };
        }
    }
}