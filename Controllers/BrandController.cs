// Controllers/BrandController.cs

using MomExchange.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MomExchange.Controllers
{
    public class BrandController : Controller
    {
        // GET: Brand
        public ActionResult Index()
        {
            var brands = GetSampleBrands();
            return View(brands);
        }

        // --- Danh sách nhãn hàng giả lập ---
        private List<Brand> GetSampleBrands()
        {
            return new List<Brand>
            {
                new Brand {
                    Id = 1,
                    Name = "Bobby",
                    Description = "Tã bỉm Nhật Bản số 1 Việt Nam",
                    LogoUrl = "/images/logo-bobby.png" // Chúng ta sẽ thêm ảnh này ở bước sau
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
    }
}