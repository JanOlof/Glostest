using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glostest.ViewModels
{
    public class SortedSynonym
    {
        public SortedList<string, WordListByLanguage> SortedWordList { get; set; } = new SortedList<string, WordListByLanguage>();


        public int Id { get; set; }
        public void AddWord(Word word)
        {
            //Sök om språket redan finns i listan. Listan över språk sorteras på Key vilket = språkets namn. Blir bra i appen
            foreach (var wordLanguage in SortedWordList)
            {
                if (wordLanguage.Key == word.Language.Name)
                {
                    wordLanguage.Value.Words.Add(word);
                    return;
                }
            }
            //Inget språk hittat i listan med ord, så vi lägger till ett nytt språk
            WordListByLanguage newWordlanguage = new WordListByLanguage { Language = word.Language };
            newWordlanguage.Words.Add(word);
            SortedWordList.Add(word.Language.Name, newWordlanguage);
        }
    }
}