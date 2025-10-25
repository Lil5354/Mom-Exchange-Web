// Controllers/ProductController.cs
using B_M.Models;
using B_M.Models.Entities;
using B_M.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace B_M.Controllers // Namespace của bạn có thể khác
{
    public class ProductController : Controller
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
        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            // Tìm sản phẩm có Id khớp với id được truyền vào
            var product = unitOfWork.Products.GetById(id);

            // Nếu không tìm thấy sản phẩm, trả về lỗi 404
            if (product == null)
            {
                return HttpNotFound();
            }

            // Nếu tìm thấy, truyền đối tượng sản phẩm (Model) cho View
            return View(product);
        }

    }
}