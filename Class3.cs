using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parfumuri;

namespace Parfumuri
{
    public class Tranzactie
    {
        public string Id { get; set; }
        public string ParfumId { get; set; }
        public string ClientId { get; set; }
        public DateTime Data { get; set; }
        public string Tip { get; set; } // vanzare sau retur adica tipul tranzactiei
        public decimal Valoare { get; set; }
    }
}
