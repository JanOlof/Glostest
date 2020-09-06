using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Glostest.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Models.User inloggning)
        {
            if (inloggning.Username == null || inloggning.Password == null)
            {
                ModelState.AddModelError("", "Du måste fylla i både användarnamn och lösenord");
                return View();
            }
            bool validUser = false;

            validUser = System.Web.Security.FormsAuthentication.Authenticate(inloggning.Username, inloggning.Password);

            if (validUser == true)
            {
                System.Web.Security.FormsAuthentication.RedirectFromLoginPage(inloggning.Username, false);
            }
            ModelState.AddModelError("", "Inloggningen ej godkänd");
            return View();
        }

        public ActionResult LogOut() 
        {
            System.Web.Security.FormsAuthentication.SignOut();
            return Redirect(Url.Content("~/"));
        }
    }
}