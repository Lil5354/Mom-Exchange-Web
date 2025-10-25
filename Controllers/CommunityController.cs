// Controllers/CommunityController.cs

using B_M.Models.Entities;
using B_M.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace B_M.Controllers
{
    public class CommunityController : Controller
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
        // GET: Community
        public ActionResult Index()
        {
            // Lấy danh sách các bài đăng từ database
            var posts = unitOfWork.SocialPosts.GetAll();

            // Truyền danh sách này đến View
            return View(posts);
        }

    }
}