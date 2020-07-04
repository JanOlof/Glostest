using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glostest.ViewModels
{
    public class SortedSynonym
    {
        public SortedList<int, WordListByLanguage> SortedWordList { get; set; } = new SortedList<int, WordListByLanguage>();
        public int Id { get; set; }
        public void AddWord(Word word)
        {
            //Sök om språket redan finns i listan
            foreach (var wordLanguage in SortedWordList)
            {
                if (wordLanguage.Key == word.LanguageId)
                {
                    wordLanguage.Value.Words.Add(word);
                    return;
                }
            }
            //Inget språk hittat i listan med ord, så vi lägger till ett nytt språk
            WordListByLanguage newWordlanguage = new WordListByLanguage { Language = word.Language };
            newWordlanguage.Words.Add(word);
            SortedWordList.Add(word.LanguageId, newWordlanguage);
        }
    }
}