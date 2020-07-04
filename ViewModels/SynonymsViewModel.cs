using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glostest.ViewModels
{
    public class SynonymsViewModel
    {
        public List<Word> WordList1 { get; set; } = new List<Word>();
        public List<Word> WordList2 { get; set; } = new List<Word>();
        public Language Language1 { get; set; }
        public Language Language2 { get; set; }
        public int Id { get; set; }
        public string NewWordText { get; set; }
        public int NewWordLanguageId { get; set; }

    }
}