// Controllers/BrandController.cs

using B_M.Models;
using B_M.Models.Entities;
using B_M.Models.ViewModels;
using B_M.Data;
using System.Collections.Generic;
using System.Linq; // Cần thêm để sử dụng .Where()
using System.Web.Mvc;

namespace B_M.Controllers // Namespace của bạn có thể khác
{
    public class BrandController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork?.Dispose();
            }
            base.Dispose(disposing);
        }
        // GET: Brand
        // Action này hiển thị trang danh sách tất cả các nhãn hàng
        public ActionResult Index()
        {
            var brands = unitOfWork.Brands.GetAll();
            return View(brands);
        }

        // GET: Brand/Details/1
        // Action này hiển thị trang chi tiết của một nhãn hàng cụ thể
        public ActionResult Details(int id)
        {
            try
            {
                // Tìm nhãn hàng dựa trên id được truyền vào
                var brand = unitOfWork.Brands.GetById(id);

                // Nếu không tìm thấy nhãn hàng, trả về lỗi 404
                if (brand == null)
                {
                    return HttpNotFound();
                }

                // Lấy các sản phẩm thuộc về nhãn hàng này với Brand navigation property
                var brandProducts = unitOfWork.Products.GetByBrandId(brand.Id)?.ToList() ?? new List<Product>();

                // Tạo ViewModel để gói dữ liệu và gửi đến View
                var viewModel = new BrandDetailViewModel
                {
                    BrandInfo = brand,
                    Products = brandProducts
                };

                return View(viewModel);
            }
            catch (System.Exception ex)
            {
                // Log error và trả về view với dữ liệu rỗng
                var viewModel = new BrandDetailViewModel
                {
                    BrandInfo = new Brand { Id = id, Name = "Unknown", Description = "Error loading brand" },
                    Products = new List<Product>()
                };
                return View(viewModel);
            }
        }

        // Child action for brand dropdown in layout
        [ChildActionOnly]
        public ActionResult BrandDropdown()
        {
            var brands = unitOfWork.Brands.GetAll();
            return PartialView("_BrandDropdown", brands);
        }

    }
}