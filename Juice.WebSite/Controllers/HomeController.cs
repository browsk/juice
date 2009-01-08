using System.Web.Mvc;

namespace Juice.WebSite.Controllers
{
    [HandleError]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Title"] = "Home Page";
            ViewData["Message"] = "Welcome to ASP.NET MVC!";

            if (Request.Cookies["CurrentProjectName"] != null)
            {
                ViewData["CurrentProjectName"] = Request.Cookies["CurrentProjectName"];
            }

            return View("Index");
        }

        public ActionResult About()
        {
            ViewData["Title"] = "About Page";

            return View();
        }
    }
}