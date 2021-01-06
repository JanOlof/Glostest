using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Glostest;

namespace Glostest.Controllers
{
    [Authorize]
    public class UsersAdminController : Controller
    {
        private WordModel db = new WordModel();

        // GET: UsersAdmin
        public ActionResult Index()
        {   
            if(CheckAdminUser())
                return View(db.User.ToList());
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: UsersAdmin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            if (CheckAdminUser())
                return View(user);
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: UsersAdmin/Create
        public ActionResult Create()
        {
            if (CheckAdminUser())
                return View();
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: UsersAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Username,Password,Name")] User user)
        {
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            if (CheckAdminUser())
                return View(user);
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: UsersAdmin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (CheckAdminUser())
                return View(user);
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: UsersAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,Name")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            if (CheckAdminUser())
                return View(user);
            else
                return RedirectToAction("Index", "Home");
        }

        // GET: UsersAdmin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (CheckAdminUser())
                return View(user);
            else
                return RedirectToAction("Index", "Home");
        }

        // POST: UsersAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.User.Find(id);
            db.User.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        private bool CheckAdminUser() 
        { 
             if (System.Web.HttpContext.Current.User.Identity.Name.ToUpper() == "ADMIN")
                return true;
            else
                return false;
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
