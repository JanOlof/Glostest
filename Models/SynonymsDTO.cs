using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glostest.Models
{
    public class SynonymsDTO
    {
        public int Id { get; set; }
        public List<WordListByLanguageDTO> WordList { get; set; } =  new List<WordListByLanguageDTO>();
    }
}