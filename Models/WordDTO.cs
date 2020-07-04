using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glostest.Models
{
    public class WordDTO
    {
        public int Id { get; set; }
         
        public string Text { get; set; }

        public int LanguageId { get; set; }

        public String Language { get; set; }
    }
}