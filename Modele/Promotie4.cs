using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Modele
{
    public class Promotie
    {
        public string Nume { get; set; }
        public decimal ProcentReducere { get; set; }
        public DateTime DataStart { get; set; }
        public DateTime DataSfarsit { get; set; }
        public string CodPromotional { get; set; } // cod promotiomal pentru o reducere
    }
}
