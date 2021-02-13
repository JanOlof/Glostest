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
    public class LanguagesController : Controller
    {
        private WordModel db = new WordModel();

        // GET: Languages
        public ActionResult Index()
        {
            if (CheckAdminUser() == false)
                return RedirectToAction("Index", "Home");

                return View(db.Language.ToList());
        }

        // GET: Languages/Details/5
        public ActionResult Details(int? id)
        {
            if (CheckAdminUser() == false)
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = db.Language.Find(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        // GET: Languages/Create
        public ActionResult Create()
        {
            if (CheckAdminUser() == false)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: Languages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Code")] Language language)
        {
            if (CheckAdminUser() == false)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Language.Add(language);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(language);
        }

        // GET: Languages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (CheckAdminUser() == false)
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = db.Language.Find(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        // POST: Languages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Code")] Language language)
        {
            if (CheckAdminUser() == false)
                return RedirectToAction("Index", "Home");

            if (ModelState.IsValid)
            {
                db.Entry(language).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(language);
        }

        // GET: Languages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (CheckAdminUser() == false)
                return RedirectToAction("Index", "Home");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Language language = db.Language.Find(id);
            if (language == null)
            {
                return HttpNotFound();
            }
            return View(language);
        }

        // POST: Languages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (CheckAdminUser() == false)
                return RedirectToAction("Index", "Home");

            Language language = db.Language.Find(id);
            db.Language.Remove(language);
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
