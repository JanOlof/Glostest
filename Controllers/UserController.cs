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
        private WordModel db = new WordModel();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(User inloggning)
        {
            if (inloggning.Username == null || inloggning.Password == null)
            {
                ModelState.AddModelError("", "Du måste fylla i både användarnamn och lösenord");
                return View();
            }
            bool validUser = false;
            validUser = CheckUser(inloggning);
            //validUser = System.Web.Security.FormsAuthentication.Authenticate(inloggning.Username, inloggning.Password);

            if (validUser == true)
            {
                System.Web.Security.FormsAuthentication.RedirectFromLoginPage(inloggning.Username, false);
            }
            ModelState.AddModelError("", "Inloggningen ej godkänd");
            return View();
        }

        private bool CheckUser(User user)
        {
            var dbUser = db.User.Where(u => u.Username == user.Username && u.Password == user.Password).FirstOrDefault();

            if (db.User != null)
            {
                Session["UserId"] = dbUser.Id;
                return true;
            }
            else
                return false;
        }

        public ActionResult LogOut() 
        {
            System.Web.Security.FormsAuthentication.SignOut();
            Session["UserId"] = null;
            return Redirect(Url.Content("~/"));
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}