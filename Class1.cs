using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parfumuri;

namespace Parfumuri
{
    public class Parfum
    {
        
        
    public string Id { get; set; }
    public string Nume { get; set; }
    public string Brand { get; set; }
    public Concentratie Concentratie { get; set; }
    public int CantitateMl { get; set; }
    public int Stoc { get; set; }
    public decimal Pret { get; set; }

    public Parfum(string id, string nume, string brand, Concentratie concentratie, int cantitateMl, int stoc, decimal pret)
    {
        Id = id;
        Nume = nume;
        Brand = brand;
        Concentratie = concentratie;
        CantitateMl = cantitateMl;
        Stoc = stoc;
        Pret = pret;
    }
}
}
