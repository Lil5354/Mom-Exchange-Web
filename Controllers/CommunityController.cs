// Controllers/CommunityController.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MomExchange.Controllers // Tên namespace của bạn có thể khác
{
    public class CommunityController : Controller
    {
        // GET: Community
        public ActionResult Index()
        {
            return View();
        }
    }
}