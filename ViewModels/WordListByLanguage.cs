using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glostest.ViewModels
{
    public class WordListByLanguage
    {
        public Language Language { get; set; }
        public List<Word> Words { get; set; } = new List<Word>();
    }
}