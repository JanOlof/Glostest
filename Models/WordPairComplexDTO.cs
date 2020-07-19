using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Glostest.Models
{
    public class WordPairComplexDTO
    {
        public int Id { get; set; }
        public List<WordDTO> Word1 { get; set; } = new List<WordDTO>();
        public List<WordDTO> Word2 { get; set; } = new List<WordDTO>();
    }
}