using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace radaway_surcharge_calc_HUN.Models
{
    [Table("TermekCsalad")]
    public class ProductFamilies
    {
        [Key]
        //[Column("TermekID")]
        public int TermekID { get; set; }
        //[Column("Termek_Csalad")]
        public string Termek_Csalad { get; set; }
        public int Uveg_Vastagsag_mm { get; set; }
    }
}
