using System;

namespace Modele
{
    public class Client
    {
        public string Id { get; set; }
        public string Nume { get; set; }
        public string Telefon { get; set; }
        public string Email { get; set; }
        public int PuncteLoialitate { get; set; }
        public DateTime DataNasterii { get; set; }
        public DateTime DataActualizare { get; set; }

        public Client(string id, string nume, string telefon, string email,
                      int puncte = 0, DateTime? dataNasterii = null)
        {
            Id = id;
            Nume = nume;
            Telefon = telefon;
            Email = email;
            PuncteLoialitate = puncte;
            DataNasterii = dataNasterii ?? DateTime.Today;
            DataActualizare = DateTime.Now;
        }

        public bool EsteValid()
        {
            return !string.IsNullOrWhiteSpace(Telefon);
        }
    }
}