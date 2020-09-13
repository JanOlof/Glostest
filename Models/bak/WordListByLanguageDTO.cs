using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glostest.Models
{
    public class WordListByLanguageDTO
    {
        public String Language  { get; set; }
        public List<WordDTO> Words { get; set; } = new List<WordDTO>();
    }
}