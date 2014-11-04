using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Web.Mvc;
using AzureAdMvcExample.Infrastructure.Auth;
using Microsoft.Azure.ActiveDirectory.GraphClient;

namespace AzureAdMvcExample.Controllers
{
    public class UserLookupController : Controller
    {
        private readonly AzureADGraphConnection _graphConnection;

        public UserLookupController()
        {
            // This should be injected in via a DI container, but for simplicity of the demo I'll leave it as a new statement
            _graphConnection = new AzureADGraphConnection(
                ConfigurationManager.AppSettings["AzureADTenant"],
                ConfigurationManager.AppSettings["ida:ClientId"],
                ConfigurationManager.AppSettings["ida:Password"]);
        }

        public ActionResult Index()
        {
            return View(new UserLookupViewModel());
        }

        [HttpPost]
        public ActionResult Index(UserLookupViewModel vm)
        {
            if (ModelState.IsValid)
                vm.User = _graphConnection.GetUser(vm.UserId.Value);
            else
                vm.UserId = null;

            return View(vm);
        }

        public ActionResult Search(string q)
        {
            var users = _graphConnection.SearchUsers(q);

            return Json(users, JsonRequestBehavior.AllowGet);
        }
    }

    public class UserLookupViewModel
    {
        private User _user;

        [Required]
        public Guid? UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [ReadOnly(true)]
        public User User
        {
            get
            {
                return _user;
            }
            set
            {
                _user = value;
                UserId = Guid.Parse(_user.ObjectId);
                UserName = _user.DisplayName;
            }
        }
    }
}