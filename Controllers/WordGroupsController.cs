﻿using System;
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
    public class WordGroupsController : Controller
    {
        private WordModel db = new WordModel();

        // GET: WordGroups
        public ActionResult Index()
        {
            int userId = 0;
            if (Session["UserId"] != null)
                userId = int.Parse(Session["UserId"].ToString()); 

            return View(db.WordGroup.Where(u => u.UserId == userId).OrderBy(d => d.Description).ToList());
        }

        // GET: WordGroups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WordGroup wordGroup = db.WordGroup.Find(id);
            if (wordGroup == null)
            {
                return HttpNotFound();
            }
            return View(wordGroup);
        }

        // GET: WordGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WordGroups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Description")] WordGroup wordGroup)
        {
            if (ModelState.IsValid)
            {
                int userId = 0;
                if (Session["UserId"] != null)
                    userId = int.Parse(Session["UserId"].ToString());
                else
                    return HttpNotFound("Error UserId for creator not found");

                wordGroup.UserId = userId;
                db.WordGroup.Add(wordGroup);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(wordGroup);
        }

        // GET: WordGroups/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WordGroup wordGroup = db.WordGroup.Find(id);
            if (wordGroup == null)
            {
                return HttpNotFound();
            }
            return View(wordGroup);
        }

        // POST: WordGroups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Description,UserId")] WordGroup wordGroup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wordGroup).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(wordGroup);
        }

        // GET: WordGroups/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WordGroup wordGroup = db.WordGroup.Find(id);
            if (wordGroup == null)
            {
                return HttpNotFound();
            }
            return View(wordGroup);
        }

        // POST: WordGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            WordGroup wordGroup = db.WordGroup.Find(id);
            var wordGroupSynonyms = db.WordGroupSynonym.Where(w => w.WordGroupId == wordGroup.Id);
            List<Synonyms> synonyms = new List<Synonyms>();


            foreach (var wordGroupSynonym in wordGroupSynonyms)
            {
                synonyms.AddRange(db.Synonyms.Where(s => s.SynonymId == wordGroupSynonym.SynonymId));
            }

            foreach (var item in synonyms)
            {
                db.Word.Remove(db.Word.Find(item.WordId));
                var dbSynonyms = db.Synonyms.Where(i => i.SynonymId == item.SynonymId);
                if(dbSynonyms != null)
                    db.Synonyms.RemoveRange(dbSynonyms);
            }
            db.WordGroupSynonym.RemoveRange(wordGroupSynonyms);
            db.WordGroup.Remove(wordGroup);
            db.SaveChanges();

            return RedirectToAction("Index");
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
