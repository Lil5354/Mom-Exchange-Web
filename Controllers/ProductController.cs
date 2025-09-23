// Controllers/ProductController.cs
using MomExchange.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MomExchange.Controllers // Namespace của bạn có thể khác
{
    public class ProductController : Controller
    {
        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            // Lấy danh sách sản phẩm giả lập
            var allProducts = GetSampleProducts();

            // Tìm sản phẩm có Id khớp với id được truyền vào
            var product = allProducts.FirstOrDefault(p => p.Id == id);

            // Nếu không tìm thấy sản phẩm, trả về lỗi 404
            if (product == null)
            {
                return HttpNotFound();
            }

            // Nếu tìm thấy, truyền đối tượng sản phẩm (Model) cho View
            return View(product);
        }

        // --- Danh sách sản phẩm giả lập (thay cho cơ sở dữ liệu) ---
        private List<Product> GetSampleProducts()
        {
            return new List<Product>
            {
                new Product {
                    Id = 1,
                    Name = "Xe đẩy em bé Aprica",
                    Category = "Xe đẩy, Nôi cũi",
                    Price = "1.200.000₫",
                    ShortDescription = "Xe đẩy Aprica nội địa Nhật, còn mới 95%, đầy đủ phụ kiện. Gấp gọn dễ dàng, phù hợp cho bé từ sơ sinh đến 3 tuổi.",
                    DetailedDescription = "Xe nhà mình dùng kỹ nên còn mới đến 95%, không có lỗi hỏng, trầy xước không đáng kể. Vải nệm đã được giặt sạch sẽ, thơm tho, bé có thể dùng ngay.",
                    Condition = "Đã sử dụng (còn mới 95%)",
                    Brand = "Aprica (Nhật Bản)",
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
                    Brand = "Medela",
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
                    Brand = "Nous",
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
                }
                // Thêm sản phẩm có id = 4 và các sản phẩm khác vào đây
            };
        }
    }
}