using System.Web.Mvc;

namespace B_M.Controllers
{
    public class ErrorController : Controller
    {
        // GET: /Error/NotFound - Main 404 page
        public ActionResult NotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        // GET: /Error/ServerError - 500 errors
        public ActionResult ServerError()
        {
            Response.StatusCode = 500;
            return View("ServerError");
        }

        // GET: /Error/Forbidden - 403 errors redirect to 404
        public ActionResult Forbidden()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        // GET: /Error/Unauthorized - 401 errors redirect to 404
        public ActionResult Unauthorized()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        // Catch-all method for any other errors
        public ActionResult Index()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }

        // Handle all unmatched routes
        public ActionResult HandleNotFound()
        {
            Response.StatusCode = 404;
            return View("NotFound");
        }
    }
}
