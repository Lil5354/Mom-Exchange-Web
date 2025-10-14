// Controllers/BrandController.cs

using B_M.Models;
using System.Collections.Generic;
using System.Linq; // Cần thêm để sử dụng .Where()
using System.Web.Mvc;

namespace B_M.Controllers // Namespace của bạn có thể khác
{
    public class BrandController : Controller
    {
        // GET: Brand
        // Action này hiển thị trang danh sách tất cả các nhãn hàng
        public ActionResult Index()
        {
            var brands = GetSampleBrands();
            return View(brands);
        }

        // GET: Brand/Details/1
        // Action này hiển thị trang chi tiết của một nhãn hàng cụ thể
        public ActionResult Details(int id)
        {
            // Tìm nhãn hàng dựa trên id được truyền vào
            var brand = GetSampleBrands().FirstOrDefault(b => b.Id == id);

            // Nếu không tìm thấy nhãn hàng, trả về lỗi 404
            if (brand == null)
            {
                return HttpNotFound();
            }

            // Lọc ra các sản phẩm thuộc về nhãn hàng này
            var brandProducts = GetSampleProducts().Where(p => p.Brand == brand.Name).ToList();

            // Tạo ViewModel để gói dữ liệu và gửi đến View
            var viewModel = new BrandDetailViewModel
            {
                BrandInfo = brand,
                Products = brandProducts
            };

            return View(viewModel);
        }

        // --- Danh sách nhãn hàng giả lập (thay cho cơ sở dữ liệu) ---
        private List<Brand> GetSampleBrands()
        {
            return new List<Brand>
            {
                new Brand {
                    Id = 1,
                    Name = "Bobby",
                    Description = "Tã bỉm Nhật Bản số 1 Việt Nam",
                    LogoUrl = "/images/logo-bobby.png"
                },
                new Brand {
                    Id = 2,
                    Name = "Huggies",
                    Description = "Thoải mái cho bé, an tâm cho mẹ",
                    LogoUrl = "/images/logo-huggies.png"
                },
                new Brand {
                    Id = 3,
                    Name = "Moony",
                    Description = "Tã dán siêu mềm, siêu thấm hút",
                    LogoUrl = "/images/logo-moony.png"
                },
                new Brand {
                    Id = 4,
                    Name = "Aptamil",
                    Description = "Dinh dưỡng chuyên sâu từ Anh Quốc",
                    LogoUrl = "/images/logo-aptamil.png"
                },
                new Brand {
                    Id = 5,
                    Name = "Pigeon",
                    Description = "Sản phẩm chăm sóc mẹ và bé toàn diện",
                    LogoUrl = "/images/logo-pigeon.png"
                },
                new Brand {
                    Id = 6,
                    Name = "Comotomo",
                    Description = "Bình sữa silicon cao cấp cho bé",
                    LogoUrl = "/images/logo-comotomo.png"
                },
                new Brand {
                    Id = 7,
                    Name = "BioGaia",
                    Description = "Men vi sinh cho hệ tiêu hóa khỏe mạnh",
                    LogoUrl = "/images/logo-biogaia.png"
                },
                new Brand {
                    Id = 8,
                    Name = "Chicco",
                    Description = "Thương hiệu mẹ và bé từ Ý",
                    LogoUrl = "/images/logo-chicco.png"
                }
            };
        }

        // --- Danh sách sản phẩm giả lập (thay cho cơ sở dữ liệu) ---
        private List<Product> GetSampleProducts()
        {
            return new List<Product>
            {
                new Product { Id = 10, Name = "Tã dán Bobby size M", Brand = "Bobby", Price = "185.000₫", ImageUrls = new List<string> { "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRlw-3TTe4si5ZpUNLulfDWkUK6wy9NkXnfqA&s" }, SellerName = "Nhà phân phối chính hãng" },
                new Product { Id = 11, Name = "Tã quần Bobby size L", Brand = "Bobby", Price = "210.000₫", ImageUrls = new List<string> { "https://cdn-v2.kidsplaza.vn/media/catalog/product/b/i/bim-ta-quan-bobby-fresh-size-l-68-mieng-cho-be-9-13kg-a.jpg" }, SellerName = "Nhà phân phối chính hãng" },
                new Product { Id = 12, Name = "Tã dán Huggies Platinum", Brand = "Huggies", Price = "350.000₫", ImageUrls = new List<string> { "https://www.huggies.com.vn/-/media/Project/HuggiesVN/Images/Product/ta-dan-huggies-platinum-naturemade/ta-dan-huggies-platinum-naturemade-size-xl-pdp-image.jpg" }, SellerName = "Nhà phân phối chính hãng" },
                new Product { Id = 13, Name = "Sữa Aptamil Profutura 1", Brand = "Aptamil", Price = "650.000₫", ImageUrls = new List<string> { "https://concung.com/2024/05/64564-110429-large_mobile/aptamil-profutura-cesarbiotik-1-800g-0-12-thang.png" }, SellerName = "Nhà phân phối chính hãng" },
            };
        }
    }
}