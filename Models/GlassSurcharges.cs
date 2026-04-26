using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace radaway_surcharge_calc_HUN.Models
{
    [Table("UvegFelar")]
    public class GlassSurcharges
    {
        [Key]
        public int FelarID { get; set; }
        public string Uveg_Tipus { get; set; }
        public int Uveg_Vastagsag_mm { get; set; }
        public int Felar_ft { get; set; }
    }
}

