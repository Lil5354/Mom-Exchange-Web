// Controllers/TradeController.cs
using B_M.Models;
using B_M.Models.Entities;
using B_M.Data;
using System.Collections.Generic;
using System.Web.Mvc;

namespace B_M.Controllers
{
    public class TradeController : Controller
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
        // GET: Trade
        public ActionResult Index()
        {
            var tradePosts = unitOfWork.TradePosts.GetAll();
            return View(tradePosts);
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