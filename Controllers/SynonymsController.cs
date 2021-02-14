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

        private int GetNewSynonymsId()
        {
            int maxId = 1;
            int antal = db.Synonyms.Count();// Max(i => i.SynonymId).first;
            if (antal != 0)
                maxId = db.Synonyms.Max(i => i.SynonymId) + 1;
            
            return maxId;
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
