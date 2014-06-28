using System;
using System.IdentityModel.Services;
using System.Web.Mvc;

namespace AzureAdMvcExample.Controllers
{
    public class LogoutController : Controller
    {
        public ActionResult Index()
        {
            var config = FederatedAuthentication.FederationConfiguration.WsFederationConfiguration;

            var callbackUrl = Url.Action("Callback", "Logout", null, Request.Url.Scheme);
            var signoutMessage = new SignOutRequestMessage(new Uri(config.Issuer), callbackUrl);
            signoutMessage.SetParameter("wtrealm", config.Realm);
            FederatedAuthentication.SessionAuthenticationModule.SignOut();

            return new RedirectResult(signoutMessage.WriteQueryString());
        }

        [AllowAnonymous]
        public ActionResult Callback()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}