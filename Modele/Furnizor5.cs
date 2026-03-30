using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Modele
{
    public class Furnizor
    {
        public string NumeCompanie { get; set; }
        public string ContactPersoana { get; set; }
        public string EmailOficial { get; set; }
        public List<string> BranduriDistribuite { get; set; }
    }
}
