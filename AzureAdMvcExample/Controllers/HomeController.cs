using System.Web.Mvc;
using AzureAdMvcExample.Infrastructure.Auth;

namespace AzureAdMvcExample.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeRoles(AppRoles.Group1)]
        public ActionResult Group1()
        {
            return View();
        }

        [AuthorizeRoles(AppRoles.Group2)]
        public ActionResult Group2()
        {
            return View();
        }

        [AuthorizeRoles(AppRoles.Group3)]
        public ActionResult Group3()
        {
            return View();
        }
    }
}