using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Glostest;
using Glostest.ViewModels;
using WebGrease.Css.Extensions;

namespace Glostest.Controllers
{
    [Authorize]
    public class SynonymsController : Controller
    {
        private WordModel db = new WordModel();

        public ActionResult Index(int wordGroupId = 0)
        {
            if (wordGroupId == 0)
                return RedirectToAction("Index", "WordGroups");

            Session["WordGroupId"] = wordGroupId;
            SynonymsView viewModel = new SynonymsView();//FillViewModel();
            viewModel.FillViewModel(wordGroupId);
            ViewBag.LanguageId = new SelectList(db.Language, "Id", "Name");
          
            return View(viewModel);
        }
 
        public ActionResult NewWord(SynonymsView synonymsView, int LanguageId, int SynonymId)
        {
            int wordGroupId = 0;
            if (!int.TryParse(Session["WordGroupId"].ToString(), out wordGroupId))
                throw new Exception("Hittar inget grupp Id och kan inte spara ny synonym i gruppen");

            Word word = new Word();
            word.LanguageId = LanguageId;
            word.Text = synonymsView.NewWordText;
            db.Word.Add(word);
            db.SaveChanges();
            Synonyms synonym = null;
            synonym = new Synonyms();
            if (SynonymId == 0) //Vi lägger till en helt ny grupp synonymer
            { 
                synonym.SynonymId = GetNewSynonymsId();
                db.WordGroupSynonym.Add(new WordGroupSynonym { SynonymId = synonym.SynonymId, WordGroupId = wordGroupId });
                    db.SaveChanges();
            }
            else
                synonym.SynonymId = SynonymId;

            synonym.Word = word;
            db.Synonyms.Add(synonym);
            db.SaveChanges();

            var routeValuesNewQuestion = new RouteValueDictionary();
            routeValuesNewQuestion.Add("wordGroupId", wordGroupId.ToString());
            return RedirectToAction("Index", routeValuesNewQuestion);

        }

        // GET: Synonyms
        public ActionResult IndexOld()
        {
            var synonyms = db.Synonyms.Include(s => s.Word);
            return View(synonyms.ToList());
        }

        // GET: Synonyms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Synonyms synonyms = db.Synonyms.Find(id);
            if (synonyms == null)
            {
                return HttpNotFound();
            }
            return View(synonyms);
        }

        // GET: Synonyms/Create
        public ActionResult Create()
        {
            //ToDo ta bort hårdkondningen och lagra valt språk i sessionen
            ViewBag.WordId1 = new SelectList(db.Word , "Id", "Text");
            ViewBag.WordId2 = new SelectList(db.Word , "Id", "Text");
            return View();
        }

        // POST: Synonyms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateWordPairView newSynonym)
        {
            if (ModelState.IsValid)
            {
                int sysnomymId1 = 0;
                Synonyms synonyms1 = null;
                Synonyms dbSynonymForWord1 = db.Synonyms.Where(w => w.WordId == newSynonym.WordId1).FirstOrDefault();
                if (dbSynonymForWord1 == null)
                {
                    int newId = GetNewSynonymsId();
                    synonyms1 = new Synonyms {WordId = newSynonym.WordId1, SynonymId = newId };
                    db.Synonyms.Add(synonyms1);
                    db.SaveChanges();
                    sysnomymId1 = synonyms1.SynonymId;
                }
                else
                {
                    sysnomymId1 = dbSynonymForWord1.SynonymId;
                }
                Synonyms dbSynonymForWord2 = db.Synonyms.Where(w => w.WordId == newSynonym.WordId2).FirstOrDefault();
                
                if (dbSynonymForWord2 == null) 
                { 
                    Synonyms synonyms2 = new Synonyms {SynonymId = sysnomymId1, WordId = newSynonym.WordId2 };
                    db.Synonyms.Add(synonyms2);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            //ViewBag.WordId = new SelectList(db.Word, "Id", "Text", synonyms.WordId);
            //ToDo ta bort hårdkondningen och lagra valt språk i sessionen
            ViewBag.WordId1 = new SelectList(db.Word.Where(m => m.LanguageId == 1), "Id", "Text");
            ViewBag.WordId2 = new SelectList(db.Word.Where(m => m.LanguageId == 2), "Id", "Text");
            return View(newSynonym);
        }

        private int GetNewSynonymsId()
        {
            int maxId = 1;
            int antal = db.Synonyms.Count();// Max(i => i.SynonymId).first;
            if (antal != 0)
                maxId = db.Synonyms.Max(i => i.SynonymId) + 1;
            
            return maxId;
        }

        // GET: Synonyms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Synonyms synonyms = db.Synonyms.Find(id);
            if (synonyms == null)
            {
                return HttpNotFound();
            }
            ViewBag.WordId = new SelectList(db.Word, "Id", "Text", synonyms.WordId);
            return View(synonyms);
        }

        // POST: Synonyms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SynonymId,WordId")] Synonyms synonyms)
        {
            if (ModelState.IsValid)
            {
                db.Entry(synonyms).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.WordId = new SelectList(db.Word, "Id", "Text", synonyms.WordId);
            return View(synonyms);
        }

        // GET: Synonyms/Delete/5
        public ActionResult Delete(int wordId, int synonymId)
        {
            var synonym = db.Synonyms.Where(s => s.WordId == wordId && s.SynonymId == synonymId).FirstOrDefault();
            db.Synonyms.Remove(synonym);
            db.SaveChanges();
            Word word = db.Word.Find(wordId);
            db.Word.Remove(word);
            db.SaveChanges();
            var count = db.Synonyms.Where(s => s.SynonymId == synonymId).Count();
            //Ta bort från tabellen WordGroupSynonym om det inte finns några ord kvar
            if (count == 0)
            {
                var wordGroupSynonym = db.WordGroupSynonym.Where(s => s.SynonymId == synonymId);
                foreach (var item in wordGroupSynonym)
                {
                    db.WordGroupSynonym.Remove(item);
                }
                db.SaveChanges();
            }

            int wordGroupId = 0;
            if (!int.TryParse(Session["WordGroupId"].ToString(), out wordGroupId))
                throw new Exception("Hittar inget grupp Id och kan inte spara ny synonym i gruppen");

            var routeValuesNewQuestion = new RouteValueDictionary();
            routeValuesNewQuestion.Add("wordGroupId", wordGroupId.ToString());
            return RedirectToAction("Index", routeValuesNewQuestion);
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
