// Controllers/BlogController.cs
using B_M.Models;
using B_M.Models.Entities;
using B_M.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace B_M.Controllers
{
    public class BlogController : Controller
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
            var posts = unitOfWork.CommunityPosts.GetAll();
            return View(posts);
        }

        // GET: Community/Details/1
        public ActionResult Details(int id)
        {
            var post = unitOfWork.CommunityPosts.GetById(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

    }
}