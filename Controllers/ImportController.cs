using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Glostest.Controllers
{
    [Authorize]
    public class ImportController : Controller
    {
        private WordModel db = new WordModel();
        // GET: Import
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult IndexUpload()
        {

            try
            {
                int rowNumber = 0;

                if (Request.Files.Count > 0)
                {

                    var file = Request.Files[0];

                    if (file != null && file.ContentLength > 0)
                    {

                        BinaryReader b = new BinaryReader(file.InputStream);
                        int length = int.Parse(file.InputStream.Length.ToString());
                        byte[] binData = b.ReadBytes(length);

                        string result = System.Text.Encoding.UTF8.GetString(binData);
                        //string result = System.Text.Encoding.Default.GetString(binData);

                        var textArr = result.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        int userId = 0;
                        if (Session["UserId"] != null)
                            userId = int.Parse(Session["UserId"].ToString());

                        foreach (var row in textArr)
                        {
                            if (rowNumber > 0) //First row is header
                            {
                                try
                                {
                                    SaveWordData(row, userId);
                                }
                                catch (Exception e)
                                {
                                    throw new Exception("Rad: " + rowNumber.ToString() + " kunde inte importeras. Importen avbruten. Innehållet i raden: " + row + ". " + e.Message);
                                }

                            }
                            rowNumber++;
                        }
                    }
                }
                ViewBag.ImportResult = "Importen färdig. " + rowNumber.ToString() + " glosor importerades";
                return View();
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "Importen misslyckades. Felmeddelande: " + e.Message);
                return View();
            }
        }

        private void SaveWordData(string row, int userId)
        {
            //WordGroupName,NewSynonym(1=ja,0=samma),LanguageCode,Word
            string[] dataArr = row.Split(';');
            string wordGroupName = dataArr[0];
            bool newSynonym = false;
            if (dataArr[1] == "1")
                newSynonym = true;

            int wordGroupId = GetWordGroup(userId, wordGroupName);
            int currentSynonymId = GetSynonymId(newSynonym);
            int wordId = SaveNewWord(dataArr[2], dataArr[3]);
            SaveNewSynonym(currentSynonymId, wordId, wordGroupId);
        }

        private int GetSynonymId(bool newSynonym)
        {
            int currentSynonymId;
            if (newSynonym)
            {
                currentSynonymId = GetNewSynonymsId();
                Session["CurrentSynonymId"] = currentSynonymId;
            }
            else
            {
                if (Session["CurrentSynonymId"] != null)
                    currentSynonymId = int.Parse(Session["CurrentSynonymId"].ToString());
                else
                    throw new Exception("Hittar inte rätt synomymId");
            }

            return currentSynonymId;
        }

        private int GetWordGroup(int userId, string wordGroupName)
        {
            WordGroup wordGroup = db.WordGroup.Where(n => n.Description == wordGroupName && n.UserId == userId).FirstOrDefault();
            if (wordGroup == null)
            {
                wordGroup = new WordGroup { Description = wordGroupName, UserId = userId };
                db.WordGroup.Add(wordGroup);
                db.SaveChanges();
            }
            return wordGroup.Id;
        }

        private void SaveNewSynonym(int synonymId, int wordId, int wordGroupId)
        {
            Synonyms synonym = new Synonyms { SynonymId = synonymId, WordId = wordId };
            db.Synonyms.Add(synonym);

            WordGroupSynonym wordGroupSynonym = db.WordGroupSynonym.Where(s => s.SynonymId == synonymId && s.WordGroupId == wordGroupId).FirstOrDefault();
            if(wordGroupSynonym == null)
            { 
                wordGroupSynonym = new WordGroupSynonym { SynonymId = synonymId, WordGroupId = wordGroupId };
                db.WordGroupSynonym.Add(wordGroupSynonym);
            }
            db.SaveChanges();
        }
        private int SaveNewWord(string languageCode, string wordText)
        {
            Language language = db.Language.Where(l => l.Code.ToUpper() == languageCode.ToUpper()).FirstOrDefault();
            if(language == null)
                throw new Exception("Hittar inte rätt språk till: " + languageCode + " " + wordText);

            Word word = new Word { LanguageId = language.Id, Text = wordText };
            db.Word.Add(word);
            db.SaveChanges();
            return word.Id;
        }

        private int GetNewSynonymsId()
        {
            int maxId = 1;
            int antal = db.Synonyms.Count();// Max(i => i.SynonymId).first;
            if (antal != 0)
                maxId = db.Synonyms.Max(i => i.SynonymId) + 1;

            return maxId;
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