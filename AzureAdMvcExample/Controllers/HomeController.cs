using System.Web.Mvc;

namespace AzureAdMvcExample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Group 1")]
        public ActionResult Group1()
        {
            return View();
        }

        [Authorize(Roles = "Group 2")]
        public ActionResult Group2()
        {
            return View();
        }

        [Authorize(Roles = "Group 3")]
        public ActionResult Group3()
        {
            return View();
        }
    }
}