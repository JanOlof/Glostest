using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glostest.ViewModels
{
    public class SynonymsView
    {
        public List<SortedSynonym> SortedSynonyms { get; set; } = new List<SortedSynonym>();
        public string NewWordText { get; set; }
        public int NumberOfLanguages { get; set; }
        public string WordGroupName { get; set; }
        public void FillViewModel(int wordGroupId)
        {
           
            List<Language> languageCountList = new List<Language>(); //För att räkna hur många språk som finns totalt och skapa kolumner i vyn
            using (WordModel db = new WordModel())
            {
                this.WordGroupName = db.WordGroup.Find(wordGroupId).Description;

                //Hämtar en lista med id för alla synonymer som har sökt WordGroupId (borde man kanske kunna modellerat bättre...)
                var dbWordGroupWords = db.WordGroupSynonym.Where(w => w.WordGroupId == wordGroupId).Select(i => i.SynonymId).ToList();

                //Hämtar alla synonymer med tillhörande ord som finns i listan över sökt wordgroup  
                var dbSynonymList = db.Synonyms.Where(i => dbWordGroupWords.Contains(i.SynonymId)).OrderBy(i => i.SynonymId).ToList();


                int currentSynonymId = 0;
                SortedSynonym currentSynonym = null;

                //Skapa en lista som man kan returnera för display och API med alla synonymer och tillhörande ord
                foreach (var synonym in dbSynonymList)
                {
                    if (synonym.SynonymId != currentSynonymId)
                    {
                        currentSynonym = new SortedSynonym();
                        this.SortedSynonyms.Add(currentSynonym);
                        currentSynonym.Id = synonym.SynonymId;
                        currentSynonymId = synonym.SynonymId;
                    }
                    currentSynonym.AddWord(synonym.Word);

                    if (!languageCountList.Contains(synonym.Word.Language))
                        languageCountList.Add(synonym.Word.Language);
                }
                this.NumberOfLanguages = languageCountList.Count;
            }
           
        }
    }
}