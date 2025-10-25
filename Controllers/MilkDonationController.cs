// Controllers/MilkDonationController.cs
using B_M.Models;
using B_M.Models.Entities;
using B_M.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace B_M.Controllers
{
    public class MilkDonationController : Controller
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
        // GET: MilkDonation
        public ActionResult Index()
        {
            var posts = unitOfWork.MilkDonationPosts.GetAll();
            return View(posts);
        }

        // GET: MilkDonation/Details/1
        public ActionResult Details(int id)
        {
            var post = unitOfWork.MilkDonationPosts.GetById(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

    }
}