// File: Controllers/AccountController.cs
using MomExchange.Models;
using System.Web.Mvc;

namespace MomExchange.Controllers
{
    public class AccountController : Controller
    {
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Logic đăng nhập thật sẽ ở đây
            // ...
            // Giả lập đăng nhập thành công và chuyển hướng về trang chủ
            return RedirectToAction("Index", "Home");
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Logic đăng ký thật sẽ ở đây
                // ...
                // Giả lập đăng ký thành công và chuyển hướng về trang chủ
                return RedirectToAction("Index", "Home");
            }
            // Nếu có lỗi, quay lại form với các lỗi được hiển thị
            // Để giữ nguyên tab đăng ký, ta truyền một flag qua ViewBag
            ViewBag.ShowRegister = true;
            return View("Login", model);
        }
    }
}