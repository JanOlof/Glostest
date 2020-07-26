namespace Glostest
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WordTest")]
    public partial class WordTest
    {
        public int Id { get; set; }

        public int SynonymId { get; set; }
    }
}
