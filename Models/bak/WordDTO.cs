using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glostest.Models
{
    public class WordDTO
    {
        public int Id { get; internal set; }
        public string Text { get; internal set; }
        public string Language { get; internal set; }
        public int LanguageId { get; internal set; }
    }
}