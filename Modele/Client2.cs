using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Modele
{
    public class Client
    {
        public string Id { get; set; }
        public string Nume { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public int PuncteLoialitate { get; set; }

        public Client(string id, string nume, string telefon, string email, int puncte = 0)
        {
            Id = id;
            Nume = nume;
            Telefon = telefon;
            Email = email;
            PuncteLoialitate = puncte;
        }

        // validare daca nr de telefon a fost introdus
        public bool EsteValid()
        {
            // sa nu fie spatiu gol la nr de telefon

            return !string.IsNullOrWhiteSpace(Telefon);
        }
    }
}
